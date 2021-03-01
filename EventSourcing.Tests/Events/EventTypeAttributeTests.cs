using System.Reflection;
using LightestNight.EventSourcing.Events;
using Shouldly;
using Xunit;

namespace LightestNight.System.EventSourcing.Tests.Events
{
    public class EventTypeAttributeTests
    {
        [EventType]
        private class EventTypeTestClassNoParams
        { }

        [EventType(10)]
        private class EventTypeTestClassVersion10
        { }

        [EventType("TestEventType")]
        private class EventTypeGivenEventType
        { }

        [Fact]
        public void ShouldSetVersionTo0WhenNotSpecificallySet()
        {
            // Arrange
            var attributeObject = new EventTypeTestClassNoParams();
            
            // Act
            var attribute = attributeObject.GetType().GetCustomAttribute(typeof(EventTypeAttribute)) as EventTypeAttribute;
            var version = attribute?.Version;

            // Assert
            version.ShouldBe(0);
        }
        
        [Fact]
        public void VersionShouldBeAsSet()
        {
            // Arrange
            var attributeObject = new EventTypeTestClassVersion10();
            
            // Act
            var attribute = attributeObject.GetType().GetCustomAttribute(typeof(EventTypeAttribute)) as EventTypeAttribute;
            var version = attribute?.Version;

            // Assert
            version.ShouldBe(10);
        }

        [Fact]
        public void ShouldGetEventTypeFromAttribute()
        {
            // Act
            var eventType = EventTypeAttribute.GetEventTypeFrom(typeof(EventTypeGivenEventType));
            
            // Assert
            eventType.ShouldBe("TestEventType");
        }
    }
}