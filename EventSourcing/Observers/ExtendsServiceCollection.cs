using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LightestNight.System.EventSourcing.Observers.Rules;
using LightestNight.System.Utilities.Extensions;
using Microsoft.Extensions.DependencyInjection;

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
                WireEventRules(assembly, services);
                WireEventObservers(assembly, services);
            });

            return services;
        }

        private static void WireEventRules(Assembly assembly, IServiceCollection services)
        {
            var eventRuleType = typeof(IEventRule);
            var eventRules = assembly.GetInstancesOfInterface(eventRuleType);
            Parallel.ForEach(eventRules, eventRule => { services.AddTransient(eventRuleType, eventRule); });
        }

        private static void WireEventObservers(Assembly assembly, IServiceCollection services)
        {
            var eventObservers = assembly.GetInstancesOfInterface<IEventObserver>();
            Parallel.ForEach(eventObservers, eventObserver => services.AddTransient(typeof(IEventObserver), eventObserver));

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