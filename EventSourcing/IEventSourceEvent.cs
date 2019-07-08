using System;

namespace LightestNight.System.EventSourcing
{
    public interface IEventSourceEvent
    {
        /// <summary>
        /// The Globally Unique Identifier of the Aggregate this event is raised for
        /// </summary>
        Guid Id { get; set; }
    }
}