using System.Collections.Generic;
using System.Linq;
using LightestNight.System.EventSourcing.Dispatch;
using LightestNight.System.EventSourcing.Events;
using LightestNight.System.Utilities.Extensions;

// ReSharper disable MemberCanBeProtected.Global

namespace LightestNight.System.EventSourcing.Domain
{
    public abstract class EventSourceAggregate<TId> : IEventSourceAggregate<TId>
    {
        private readonly List<EventSourceEvent> _uncommittedEvents = new List<EventSourceEvent>();

        /// <inheritdoc cref="IEventSourceAggregate{TId}.Id" />
        public TId Id { get; set; } = default!;
        
        /// <inheritdoc cref="IEventSourceAggregate{TId}.Version" />
        public int Version { get; private set; }

        /// <summary>
        /// Denotes whether the Aggregate has been instantiated with no events.
        /// </summary>
        /// <remarks>
        /// Think of this as null. The aggregate will only be in this state if it's been retrieved but no events have been processed. That's basically
        /// non existent
        /// </remarks>
        public bool IsRaw => Version == 0;
        
        protected EventSourceAggregate() {}

        protected EventSourceAggregate(IEnumerable<EventSourceEvent> events)
        {
            var enumeratedEvents = events as EventSourceEvent[] ?? events.ToArray();
            
            if (enumeratedEvents.IsNullOrEmpty())
                return;
            
            foreach (var @event in enumeratedEvents)
                Apply(@event);
        }

        /// <inheritdoc cref="IEventSourceAggregate{TId}.GetUncommittedEvents" />
        public IEnumerable<EventSourceEvent> GetUncommittedEvents()
            => _uncommittedEvents;

        /// <inheritdoc cref="IEventSourceAggregate{TId}.ClearUncommittedEvents" />
        public void ClearUncommittedEvents()
        {
            _uncommittedEvents.Clear();
        }

        /// <summary>
        /// Applies the given event to the Aggregate
        /// </summary>
        /// <param name="e">The event to apply</param>
        private void Apply(EventSourceEvent e)
        {
            Version++;
            RedirectToWhen.InvokeEventOptional(this, e);
        }

        /// <summary>
        /// Publishes an event to the Aggregate to be raised and/or stored
        /// </summary>
        /// <remarks>Storage of events is an abstract concern and not to do with the Aggregate itself</remarks>
        /// <param name="e">The event to publish</param>
        protected void Publish(EventSourceEvent e)
        {
            _uncommittedEvents.Add(e);
            Apply(e);
        }
    }
}