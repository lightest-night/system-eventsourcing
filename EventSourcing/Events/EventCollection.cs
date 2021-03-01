using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LightestNight.System.Utilities;
using LightestNight.System.Utilities.Extensions;

namespace LightestNight.EventSourcing.Events
{
    internal class EventSourceEventEqualityComparer : IEqualityComparer<Type>
    {
        public bool Equals(Type? event1, Type? event2)
        {
            if (event1 == null && event2 == null)
                return true;

            if (event1 == null || event2 == null)
                return false;
            
            var eventTypeAttribute1 = EventTypeAttribute.GetEventTypeFrom(event1);
            var eventTypeAttribute2 = EventTypeAttribute.GetEventTypeFrom(event1);

            var version1 = Attributes.GetCustomAttributeValue<EventTypeAttribute, int>(event1, attribute => attribute.Version);
            var version2 = Attributes.GetCustomAttributeValue<EventTypeAttribute, int>(event2, attribute => attribute.Version);
            
            return eventTypeAttribute1.Equals(eventTypeAttribute2, StringComparison.InvariantCultureIgnoreCase) &&
                   version1.Equals(version2);
        }

        public int GetHashCode(Type eventType)
        {
            var eventTypeAttribute = EventTypeAttribute.GetEventTypeFrom(eventType);
            var attribute = eventType.GetCustomAttribute(typeof(EventTypeAttribute)) as EventTypeAttribute;
            var version = attribute?.Version ?? -1;

            var hashCode = eventTypeAttribute.GetHashCode() ^ version;
            return hashCode.GetHashCode();
        }
    }
    
    public static class EventCollection
    {
        public static IEnumerable<Type> EventTypes { get; private set; }

        static EventCollection()
        {
            EventTypes = Enumerable.Empty<Type>();
        }
        
        public static void AddAssemblyTypes(params Assembly[] assemblies)
        {
            if (assemblies.IsNullOrEmpty())
                return;
            
            var equalityComparer = new EventSourceEventEqualityComparer();
            var eventTypes = EventTypes.ToList();
            
            var assemblyEvents = assemblies.SelectMany(assembly => assembly.GetExportedTypes()).Where(type =>
                    type.GetCustomAttributes(typeof(EventTypeAttribute), true).Any() &&
                    type.BaseType == typeof(EventSourceEvent))
                .ToArray();

            eventTypes.AddRange(assemblyEvents
                .Where(assemblyEvent => assemblyEvent != null)
                .Where(assemblyEvent => !EventTypes.Contains(assemblyEvent, equalityComparer)));

            EventTypes = eventTypes;
        }

        public static Type? GetEventType(string name, int version)
            => EventTypes.SingleOrDefault(type =>
            {
                var typeName = EventTypeAttribute.GetEventTypeFrom(type);
                var typeVersion =
                    Attributes.GetCustomAttributeValue<EventTypeAttribute, int>(type, attribute => attribute.Version);

                return string.Equals(typeName, name, StringComparison.InvariantCultureIgnoreCase) &&
                       version.Equals(typeVersion);
            });
    }
}