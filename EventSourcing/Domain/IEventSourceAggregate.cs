using System;
using System.Collections.Generic;
using LightestNight.System.EventSourcing.Events;

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
        int Version { get; }

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