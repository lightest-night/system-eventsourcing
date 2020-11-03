using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LightestNight.System.EventSourcing.Events;
using LightestNight.System.EventSourcing.Observers;
using Shouldly;
using Xunit;

namespace LightestNight.System.EventSourcing.Tests.Observers
{
    public class ObserverCollectionTests : IDisposable
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

        [Theory]
        [InlineData(2)]
        [InlineData(20)]
        [InlineData(200)]
        [InlineData(2000)]
        public async Task ShouldAddMultipleObserversInParallel(int observerCount)
        {
            // Arrange
            var observers = new List<IEventObserver>(observerCount);
            for (var i = 0; i < observerCount; i++)
                observers.Add(i % 2 == 0 ? (IEventObserver)new TestObserver(i) : new OtherObserver(i));
            
            // Act
            await Task.WhenAll(observers.Select(observer =>
                ObserverCollection.RegisterObserverAsync(observer, CancellationToken.None)));
            
            // Assert
            var observerCollectionArray = ObserverCollection.GetEventObservers().ToArray();
            observerCollectionArray.Length.ShouldBe(observerCount);
            
            // Check randomly that some of our objects are there
            var random = new Random();
            var index1 = random.Next(0, observerCount + 1);
            var index2 = random.Next(0, observerCount + 1);

            var observersArray = observers.ToArray();
            observerCollectionArray.ShouldContain(observersArray[index1]);
            observerCollectionArray.ShouldContain(observersArray[index2]);
        }

        public void Dispose()
        {
            var observers = ObserverCollection.GetEventObservers().ToArray();
            foreach (var observer in observers)
                ObserverCollection.UnregisterObserver(observer);
        }
    }
}