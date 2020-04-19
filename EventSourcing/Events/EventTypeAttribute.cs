using System;
using System.Reflection;

namespace LightestNight.System.EventSourcing.Events
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EventTypeAttribute : Attribute
    {
        public string EventType { get; } = string.Empty;
        public int Version { get; }

        /// <summary>
        /// Creates a new instance of <see cref="EventTypeAttribute" /> with the given version
        /// </summary>
        /// <param name="version">The version to set</param>
        public EventTypeAttribute(int version = 0)
        {
            Version = version;
        }

        /// <summary>
        /// Creates a new instance of <see cref="EventTypeAttribute" /> with the given event type and version
        /// </summary>
        /// <param name="eventType">Name to give the type of this event</param>
        /// <param name="version">The version to set</param>
        public EventTypeAttribute(string eventType, int version = 0)
        {
            EventType = eventType;
            Version = version;
        }

        public static string GetEventTypeFrom(Type classType)
        {
            var attribute = classType.GetCustomAttribute<EventTypeAttribute>();
            if (attribute != null && !string.IsNullOrEmpty(attribute.EventType))
                return attribute.EventType;

            return classType.Name;
        }
    }
}