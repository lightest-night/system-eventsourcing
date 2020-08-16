using System.Linq;
using LightestNight.System.EventSourcing.Events;
using Shouldly;
using Xunit;

namespace LightestNight.System.EventSourcing.Tests.Events
{
    public class EventCollectionTests
    {
        [Fact]
        public void ShouldGetCorrectTypesFromLoadedAssemblies()
        {
             // Act
             var result = EventCollection.EventTypes.ToArray();
             
             // Assert
             result.ShouldContain(typeof(TestEventType));
             result.Length.ShouldBe(1);
        }

        [Fact]
        public void ShouldGetCorrectEventType()
        {
            // Act
            var type = EventCollection.GetEventType("Test", 100);
            
            // Assert
            type?.FullName.ShouldBe(typeof(TestEventType).FullName);
        }
    }
}