using System;
using System.Collections.Generic;
using System.Linq;
using LightestNight.System.EventSourcing.Dispatch;
// ReSharper disable MemberCanBeProtected.Global

namespace LightestNight.System.EventSourcing.Domain
{
    public class EventSourceAggregate : IEventSourceAggregate
    {
        private readonly List<IEventSourceEvent> _uncommittedEvents = new List<IEventSourceEvent>();

        protected EventSourceAggregate(){}
        
        public EventSourceAggregate(IEnumerable<IEventSourceEvent> events)
        {
            foreach (var @event in events ?? Enumerable.Empty<IEventSourceEvent>())
                Apply(@event);
        }

        /// <inheritdoc cref="IEventSourceAggregate.Id" />
        public Guid Id { get; set; }
        
        /// <inheritdoc cref="IEventSourceAggregate.Version" />
        public long Version { get; private set; }

        /// <summary>
        /// Denotes whether the Aggregate has been instantiated with no events.
        /// </summary>
        /// <remarks>
        /// Think of this as null. The aggregate will only be in this state if it's been retrieved but no events have been processed. That's basically
        /// non existent
        /// </remarks>
        public bool IsRaw => Version == 0;

        /// <inheritdoc cref="IEventSourceAggregate.GetUncommittedEvents" />
        public IEnumerable<IEventSourceEvent> GetUncommittedEvents()
            => _uncommittedEvents;

        /// <inheritdoc cref="IEventSourceAggregate.ClearUncommittedEvents" />
        public void ClearUncommittedEvents()
        {
            _uncommittedEvents.Clear();
        }

        /// <summary>
        /// Applies the given event to the Aggregate
        /// </summary>
        /// <param name="e">The event to apply</param>
        private void Apply(IEventSourceEvent e)
        {
            Version++;
            RedirectToWhen.InvokeEventOptional(this, e);
        }

        /// <summary>
        /// Publishes an event to the Aggregate to be raised and/or stored
        /// </summary>
        /// <remarks>Storage of events is an abstract concern and not to do with the Aggregate itself</remarks>
        /// <param name="e">The event to publish</param>
        protected void Publish(IEventSourceEvent e)
        {
            _uncommittedEvents.Add(e);
            Apply(e);
        }
    }
}