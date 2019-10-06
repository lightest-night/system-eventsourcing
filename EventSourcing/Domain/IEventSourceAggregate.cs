using System;
using System.Collections.Generic;

namespace LightestNight.System.EventSourcing.Domain
{
    public interface IEventSourceAggregate
    {
        /// <summary>
        /// The Globally Unique Identifier of this Aggregate
        /// </summary>
        Guid Id { get; set; }
        
        /// <summary>
        /// The current version of this Aggregate
        /// </summary>
        long Version { get; }

        /// <summary>
        /// Takes an enumerable of <see cref="IEventSourceEvent" /> and applies them all
        /// </summary>
        /// <param name="events">The events to apply</param>
        void Apply(IEnumerable<IEventSourceEvent> events);

        /// <summary>
        /// Gets any uncommitted events that have been raised within this Aggregate
        /// </summary>
        /// <returns>A collection of events</returns>
        IEnumerable<IEventSourceEvent> GetUncommittedEvents();

        /// <summary>
        /// Clears the uncommitted events within this Aggregate
        /// </summary>
        void ClearUncommittedEvents();
    }
}