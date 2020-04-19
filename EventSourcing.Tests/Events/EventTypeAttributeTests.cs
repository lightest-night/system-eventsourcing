using System.Reflection;
using LightestNight.System.EventSourcing.Events;
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
        public void Should_Set_Version_To_0_When_Not_Specifically_Set()
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
        public void Version_Should_Be_As_Set()
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
        public void Should_Get_Event_Type_From_Attribute()
        {
            // Act
            var eventType = EventTypeAttribute.GetEventTypeFrom(typeof(EventTypeGivenEventType));
            
            // Assert
            eventType.ShouldBe("TestEventType");
        }
    }
}