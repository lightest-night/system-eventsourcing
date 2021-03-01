using System.Linq;
using System.Reflection;
using LightestNight.EventSourcing.Events;
using Shouldly;
using Xunit;

namespace LightestNight.System.EventSourcing.Tests.Events
{
    public class EventCollectionTests
    {
        public EventCollectionTests()
        {
            EventCollection.AddAssemblyTypes(Assembly.GetExecutingAssembly());
        }
        
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
            type.ShouldNotBeNull();
            type?.FullName.ShouldBe(typeof(TestEventType).FullName);
        }

        [Fact]
        public void ShouldIgnoreDuplicates()
        {
            // Arrange
            var eventCount = EventCollection.EventTypes.Count();
            eventCount.ShouldBeGreaterThan(0);
            
            // Act
            EventCollection.AddAssemblyTypes(Assembly.GetExecutingAssembly());
            
            // Assert
            EventCollection.EventTypes.Count().ShouldBe(eventCount);
        }

        [Fact]
        public void ShouldIgnoreDuplicatesWithMultipleAssemblies()
        {
            // Arrange
            var eventCount = EventCollection.EventTypes.Count();
            eventCount.ShouldBeGreaterThan(0);
            
            // Act
            EventCollection.AddAssemblyTypes(Assembly.GetExecutingAssembly(), Assembly.GetExecutingAssembly(),
                Assembly.GetCallingAssembly());
            
            // Assert
            EventCollection.EventTypes.Count().ShouldBe(eventCount);
        }
    }
}