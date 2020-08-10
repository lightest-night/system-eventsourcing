using System;

namespace LightestNight.System.EventSourcing.Events
{
    public abstract class EventSourceEvent
    {
        /// <summary>
        /// Allows an event to contain a Correlation Id in order to link it to other events
        /// </summary>
        protected virtual Guid CorrelationId { get; } = Guid.NewGuid();
    }
}