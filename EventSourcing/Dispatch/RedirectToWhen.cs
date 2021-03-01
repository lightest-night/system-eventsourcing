using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace LightestNight.EventSourcing.Dispatch
{
    public static class RedirectToWhen
    {
        private static readonly MethodInfo? InternalPreserveStackTraceMethod =
            typeof(Exception).GetMethod("InternalPreserveStackTrace", BindingFlags.Instance | BindingFlags.NonPublic);

        private static class Cache<T>
        {
            // ReSharper disable once UnusedMember.Local
            // It is actually used, just done using Reflection in the InvokeEventOptional<T> method
            public static readonly Dictionary<Type, MethodInfo> Methods = typeof(T).GetMethods(BindingFlags.Public |
                                                                                               BindingFlags.NonPublic |
                                                                                               BindingFlags.Instance |
                                                                                               BindingFlags.Static |
                                                                                               BindingFlags.FlattenHierarchy)
                .Where(m => m.Name == "When")
                .Where(m => m.GetParameters().Length == 1)
                .ToDictionary(m => m.GetParameters().Single().ParameterType, m => m);
        }

        [DebuggerNonUserCode]
        public static void InvokeEventOptional<T>(T instance, object evt)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            
            bool TryGetMethod(Type key, out MethodInfo? methodInfo)
            {
                var cacheType = typeof(Cache<>);
                var constructed = cacheType.MakeGenericType(instance!.GetType());
                
                methodInfo = null;
                return constructed
                           .GetFields()
                           .FirstOrDefault(f => f.Name == "Methods")
                           ?.GetValue(constructed) is Dictionary<Type, MethodInfo> methods
                       && methods.TryGetValue(key, out methodInfo);
            }

            var type = evt.GetType();
            if (!TryGetMethod(type, out var info))
                // We don't care if this object does not consume the event given, they'll be persisted anyway, right? ;)
                return;

            try
            {
                info?.Invoke(instance, new[] {evt});
            }
            catch (TargetInvocationException ex)
            {
                InternalPreserveStackTraceMethod?.Invoke(ex.InnerException, Array.Empty<object>());

                throw ex.InnerException ?? ex;
            }
        }
    }
}