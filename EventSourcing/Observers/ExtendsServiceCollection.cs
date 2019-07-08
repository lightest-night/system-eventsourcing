using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LightestNight.System.EventSourcing.Observers.Rules;
using LightestNight.System.EventSourcing.Observers.Rules.Mappers;
using LightestNight.System.EventSourcing.Projections;
using LightestNight.System.ServiceResolution;
using LightestNight.System.Utilities.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LightestNight.System.EventSourcing.Observers
{
    public static class ExtendsServiceCollection
    {
        public static IServiceCollection AddObservers(this IServiceCollection services, params Assembly[] assemblies)
        {
            if (assemblies.IsNullOrEmpty())
                assemblies = new[] {Assembly.GetCallingAssembly()};

            var executingAssembly = Assembly.GetExecutingAssembly();
            if (!assemblies.Contains(executingAssembly))
                assemblies = new List<Assembly>(assemblies) {executingAssembly}.ToArray();

            Parallel.ForEach(assemblies, assembly =>
            {
                Parallel.ForEach(assembly.GetInstancesOfInterface<IProjectionWriterFactory>(), factoryType => { services.AddTransient(typeof(IProjectionWriterFactory), factoryType); });
                
                WireEventRuleMappers(assembly, services);
                WireEventRules(assembly, services);
                WireObserverOrchestrators(assembly, services);
                WireEventObservers(assembly, services);
            });

            return services;
        }

        private static void WireEventRuleMappers(Assembly assembly, IServiceCollection services)
        {
            var eventRuleMapperType = typeof(IEventRuleMapper<>);
            var mappers = assembly
                .GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == eventRuleMapperType))
                .ToArray();

            Parallel.ForEach(mappers, mapper =>
            {
                var mapperTypes = mapper.GetInterfaces().FirstOrDefault(i => i.GetGenericTypeDefinition() == eventRuleMapperType)
                    ?.GenericTypeArguments;

                if (mapperTypes.IsNullOrEmpty())
                    return;

                // ReSharper disable once AssignNullToNotNullAttribute - IsNullOrEmpty checks for null
                var mapperInstanceType = eventRuleMapperType.MakeGenericType(mapperTypes);
                services.AddTransient(mapperInstanceType, mapper);
            });
        }
        
        private static void WireEventRules(Assembly assembly, IServiceCollection services)
        {
            var eventRuleType = typeof(IEventRule);
            var eventRules = assembly.GetInstancesOfInterface(eventRuleType);
            Parallel.ForEach(eventRules, eventRule => { services.AddTransient(eventRuleType, eventRule); });
        }
        
        private static void WireObserverOrchestrators(Assembly assembly, IServiceCollection services)
        {
            var observerType = typeof(IObserverOrchestrator<,>);
            var observers = assembly
                .GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == observerType))
                .ToArray();

            Parallel.ForEach(observers, observer => { services.AddTransient(typeof(IHostedService), observer); });
        }

        private static void WireEventObservers(Assembly assembly, IServiceCollection services)
        {
            var eventObservers = assembly.GetInstancesOfInterface<IEventObserver>();
            var serviceProvider = services.BuildServiceProvider();
            
            var registeredProjectionWriterFactories = serviceProvider.GetServices<IProjectionWriterFactory>().ToArray();
            var serviceFactory = serviceProvider.GetService<ServiceFactory>() ?? Activator.CreateInstance;
            
            Parallel.ForEach(eventObservers, eventObserver =>
            {
                // Rule: Event Observers can only have either a parameter-less constructor, or one taking an IProjectionWriter instance
                var constructors = eventObserver.GetConstructors()
                    .Where(c => c.GetParameters().Any(p => p.ParameterType.GetGenericTypeDefinition() == typeof(IProjectionWriter<,>)) || !c.GetParameters().Any())
                    .ToArray();
                
                if (constructors.Length != 1)
                    throw new InvalidOperationException("An instance of IEventObserver can only have a parameter-less constructor, or a constructor with only one IProjectionWriter implementation.");

                var constructorParams = constructors.Single().GetParameters();
                if (constructorParams.IsNullOrEmpty())
                {
                    services.AddTransient(typeof(IEventObserver), eventObserver);
                    return;
                }

                if (registeredProjectionWriterFactories.IsNullOrEmpty())
                    return;

                var projectionWriterGenericArgs = constructorParams.Single().ParameterType.GetGenericArguments();
                var idType = projectionWriterGenericArgs[0];
                var readModelType = projectionWriterGenericArgs[1];
                Parallel.ForEach(registeredProjectionWriterFactories, projectionWriterFactory =>
                {
                    var factoryMethod = projectionWriterFactory.GetType().GetMethod(nameof(IProjectionWriterFactory.GetProjectionWriter))
                        ?.MakeGenericMethod(idType, readModelType);
                    if (factoryMethod == null)
                        return;

                    var projectionWriterInstance = factoryMethod.Invoke(projectionWriterFactory, null);
                    services.AddTransient(_ => serviceFactory(eventObserver, projectionWriterInstance) as IEventObserver);
                });
            });

            WireObserverCache(services);
        }

        private static void WireObserverCache(IServiceCollection services)
        {
            services.AddSingleton(serviceProvider => serviceProvider.GetServices<IEventObserver>().Select(o => new
                {
                    Observer = o,
                    Type = o.GetType()
                }).Select(x => new
                {
                    x.Observer,
                    MethodInfos = x.Type
                        .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        .Where(m => m.Name == "When")
                        .Where(m => m.GetParameters().Length == 1)
                }).SelectMany(x => x.MethodInfos.Select(y => new
                {
                    x.Observer,
                    MethodInfo = y,
                    y.GetParameters().First().ParameterType
                }))
                .GroupBy(x => x.ParameterType)
                .ToDictionary(g => g.Key, g => g.Select(y => new ObserverInfo
                {
                    Observer = y.Observer,
                    MethodInfo = y.MethodInfo
                }).ToArray()));
        }
    }
}