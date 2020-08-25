using System.Threading;
using System.Threading.Tasks;
using LightestNight.System.EventSourcing.Observers;
using Shouldly;
using Xunit;

namespace LightestNight.System.EventSourcing.Tests.Observers
{
    public class ObserverCollectionTests
    {
        [Fact]
        public async Task ShouldAddObserver()
        {
            // Arrange
            var observer = new TestObserver();
            
            // Act
            await ObserverCollection.RegisterObserverAsync(observer, CancellationToken.None);
            
            // Assert
            ObserverCollection.GetEventObservers().ShouldContain(observer);
        }
    }
}