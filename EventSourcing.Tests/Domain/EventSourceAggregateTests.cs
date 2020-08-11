using System;
using System.Collections.Generic;
using System.Linq;
using LightestNight.System.EventSourcing.Domain;
using LightestNight.System.EventSourcing.Events;
using Shouldly;
using Xunit;

namespace LightestNight.System.EventSourcing.Tests.Domain
{
    public class EventSourceAggregateTests
    {
        private class TestAggregate : EventSourceAggregate
        {
            public TestAggregate(IEnumerable<EventSourceEvent> events) : base(events){}
            
            public void DoSomething()
            {
                Publish(new SomethingWasDone(Guid.NewGuid()));
            }
        }

        private class SomethingWasDone : EventSourceEvent
        {
            public SomethingWasDone(Guid id)
            {
                Id = id;
            }

            public Guid Id { get; }
        }
        
        private readonly TestAggregate _sut = new TestAggregate(Enumerable.Empty<EventSourceEvent>());

        [Fact]
        public void ShouldAddEventToUncommittedEvents()
        {
            // Act
            _sut.DoSomething();
            
            // Assert
            _sut.GetUncommittedEvents().ShouldNotBeEmpty();
        }

        [Fact]
        public void ShouldBumpVersion()
        {
            // Arrange
            var version = _sut.Version;
            
            // Act
            _sut.DoSomething();
            
            // Assert
            _sut.Version.ShouldBeGreaterThan(version);
        }

        [Fact]
        public void ShouldClearUncommittedEvents()
        {
            // Arrange
            _sut.DoSomething();
            
            // Act
            _sut.ClearUncommittedEvents();
            
            // Assert
            _sut.GetUncommittedEvents().ShouldBeEmpty();
        }
    }
}