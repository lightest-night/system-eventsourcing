using System;
using LightestNight.System.EventSourcing.Events;

namespace LightestNight.System.EventSourcing.Tests.Events
{
    [EventType("Test", 100)]
    public class TestEventType : EventSourceEvent
    {
        public TestEventType(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}