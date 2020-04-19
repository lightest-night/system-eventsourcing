using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LightestNight.System.Utilities;

namespace LightestNight.System.EventSourcing.Events
{
    public delegate Type[] GetEventTypes();
    
    public static class EventCollection
    {
        public static Type? GetEventType(this IEnumerable<Type> eventTypes, string name, int version)
            => eventTypes.SingleOrDefault(type =>
            {
                var typeName = EventTypeAttribute.GetEventTypeFrom(type);
                var typeVersion = Attributes.GetCustomAttributeValue<EventTypeAttribute, int>(type, attribute => attribute.Version);

                return string.Equals(typeName, name) && version.Equals(typeVersion);
            });

        public static Type[] GetEventTypes(IEnumerable<Assembly> assemblies)
        {
            var events = assemblies.SelectMany(assembly => assembly.GetExportedTypes())
                .Where(type => type.GetCustomAttributes(typeof(EventTypeAttribute), true).Any() &&
                               typeof(IEventSourceEvent).IsAssignableFrom(type))
                .ToArray();

            var dupes = GetDupes(events).Where(dupe =>
                    dupe.Count > 1 && dupe.Version.GroupBy(version => version).Any(group => group.Count() > 1))
                .ToArray();
            if (dupes.Any())
                throw new InvalidOperationException($"Multiple Event Types with the same version found. {string.Join(", ", dupes.Select(dupe => dupe.Name))}");

            return events;
        }

        private static IEnumerable<(string Name, IEnumerable<int> Version, int Count)> GetDupes(
            IEnumerable<Type> events)
            => events.GroupBy(EventTypeAttribute.GetEventTypeFrom)
                .Where(group => group.Count() > 1)
                .Select(grouping => (
                    grouping.Key,
                    grouping.Select(value =>
                        ((EventTypeAttribute) value.GetCustomAttribute(typeof(EventTypeAttribute))).Version),
                    grouping.Count()));
    }
}