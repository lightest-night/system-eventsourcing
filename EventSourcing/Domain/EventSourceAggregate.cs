using System.Collections.Generic;
using System.Linq;
using LightestNight.EventSourcing.Dispatch;
using LightestNight.EventSourcing.Events;
using LightestNight.System.Utilities.Extensions;

// ReSharper disable MemberCanBeProtected.Global

namespace LightestNight.EventSourcing.Domain
{
    public abstract class EventSourceAggregate : IEventSourceAggregate
    {
        private readonly List<EventSourceEvent> _uncommittedEvents = new List<EventSourceEvent>();

        /// <inheritdoc cref="IEventSourceAggregate.Id" />
        public object Id { get; set; } = default!;
        
        /// <inheritdoc cref="IEventSourceAggregate.Version" />
        public int Version { get; private set; }

        /// <summary>
        /// Denotes whether the Aggregate has been instantiated with no events.
        /// </summary>
        /// <remarks>
        /// Think of this as null. The aggregate will only be in this state if it's been retrieved but no events have been processed. That's basically
        /// non existent
        /// </remarks>
        public bool IsRaw => Version == 0;
        
        /// <summary>
        /// Should be used to denote this aggregate was deleted
        /// </summary>
        public bool IsDeleted { get; set; }
        
        protected EventSourceAggregate() {}

        protected EventSourceAggregate(IEnumerable<EventSourceEvent> events)
        {
            var enumeratedEvents = events as EventSourceEvent[] ?? events.ToArray();
            
            if (enumeratedEvents.IsNullOrEmpty())
                return;
            
            foreach (var @event in enumeratedEvents)
                Apply(@event);
        }

        /// <inheritdoc cref="IEventSourceAggregate.GetUncommittedEvents" />
        public IEnumerable<EventSourceEvent> GetUncommittedEvents()
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
        private void Apply(EventSourceEvent e)
        {
            if (IsDeleted)
                throw new AggregateDeletedException(this);
            
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
            if (IsDeleted)
                throw new AggregateDeletedException(this);
            
            _uncommittedEvents.Add(e);
            Apply(e);
        }
    }
}