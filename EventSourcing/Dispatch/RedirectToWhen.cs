using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace LightestNight.System.EventSourcing.Dispatch
{
    public static class RedirectToWhen
    {
        //private const string InternalPreserveStackTraceMethodName = "InternalPreserveStackTrace";
        //private const string WhenMethodName = "When";

        private static readonly MethodInfo InternalPreserveStackTraceMethod =
            typeof(Exception).GetMethod("InternalPreserveStackTrace", BindingFlags.Instance | BindingFlags.NonPublic);

        private static class Cache<T>
        {
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
        public static void InvokeEventOptional<T>(T instance, object @event)
        {
            bool TryGetMethod(Type key, out MethodInfo methodInfo)
            {
                methodInfo = null;

                var cacheType = typeof(Cache<>);
                var constructed = cacheType.MakeGenericType(instance.GetType());

                return constructed
                           .GetFields()
                           .FirstOrDefault(f => f.Name == "Methods")
                           ?.GetValue(constructed) is Dictionary<Type, MethodInfo> methods
                       && methods.TryGetValue(key, out methodInfo);
            }

            var type = @event.GetType();
            if (!TryGetMethod(type, out var info))
                // We don't care if this object does not consume the event given, they'll be persisted anyway, right? ;)
                return;

            try
            {
                info.Invoke(instance, new[] {@event});
            }
            catch (TargetInvocationException ex)
            {
                if (InternalPreserveStackTraceMethod != null)
                    InternalPreserveStackTraceMethod.Invoke(ex.InnerException, new object[0]);

                throw ex.InnerException ?? ex;
            }
        }
    }
}