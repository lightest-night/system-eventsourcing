using System;
using System.Collections.Generic;
using System.Linq;
using LightestNight.System.EventSourcing.Domain;
using Shouldly;
using Xunit;

namespace LightestNight.System.EventSourcing.Tests.Domain
{
    public class EventSourceAggregateTests
    {
        private class TestAggregate : EventSourceAggregate
        {
            public TestAggregate(IEnumerable<IEventSourceEvent> events) : base(events) {}
            
            public void DoSomething()
            {
                Publish(new SomethingWasDone(Guid.NewGuid()));
            }
        }

        private class SomethingWasDone : IEventSourceEvent
        {
            public SomethingWasDone(Guid id)
            {
                Id = id;
            }

            public Guid Id { get; set; }
        }
        
        private readonly TestAggregate _sut = new TestAggregate(Enumerable.Empty<IEventSourceEvent>());

        [Fact]
        public void Should_Add_Event_To_UncommittedEvents()
        {
            // Act
            _sut.DoSomething();
            
            // Assert
            _sut.GetUncommittedEvents().ShouldNotBeEmpty();
        }

        [Fact]
        public void Should_Bump_Version()
        {
            // Arrange
            var version = _sut.Version;
            
            // Act
            _sut.DoSomething();
            
            // Assert
            _sut.Version.ShouldBeGreaterThan(version);
        }

        [Fact]
        public void Should_Clear_UncommittedEvents()
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