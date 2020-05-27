using System;
using System.Reflection;
using LightestNight.System.EventSourcing.Events;
using Shouldly;
using Xunit;

namespace LightestNight.System.EventSourcing.Tests.Events
{
    public class EventCollectionTests
    {
        [Fact]
        public void ShouldGetCorrectEventType()
        {
            // Arrange
            var types = new Type[]
            {
                typeof(string),
                typeof(int),
                typeof(Guid),
                typeof(TestEventType)
            };
            
            // Act
            var type = types.GetEventType("Test", 100);
            
            // Assert
            type?.FullName.ShouldBe(typeof(TestEventType).FullName);
        }

        [Fact]
        public void ShouldGetEventTypesFromGivenAssemblies()
        {
            // Arrange
            var assembly = Assembly.GetExecutingAssembly();
            
            // Act
            var result = EventCollection.GetEventTypes(new[] {assembly});
            
            // Assert
            result.ShouldContain(typeof(TestEventType));
            result.Length.ShouldBe(1);
        }
    }
}