using System;
using LightestNight.System.EventSourcing.Events;

namespace LightestNight.System.EventSourcing.Tests.Events
{
    [EventType("Test", 100)]
    public class TestEventType : IEventSourceEvent
    {
        public Guid Id { get; set; }
    }
}