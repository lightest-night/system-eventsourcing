using LightestNight.System.EventSourcing.Dispatch;
using Shouldly;
using Xunit;

namespace LightestNight.System.EventSourcing.Tests.Dispatch
{
    public class RedirectToWhenTests
    {
        private class TestObject
        {
            public bool WhenCalled = false;

            public void FireWhen()
            {
                RedirectToWhen.InvokeEventOptional(this, new TestEvent());
            }

            private void When(TestEvent @event)
            {
                WhenCalled = true;
            }
            
            private void When(WrongTestEvent @event){}
        }

        private class TestEvent{}

        private class WrongTestEvent{}

        [Fact]
        public void Should_Dispatch_To_Correct_Event()
        {
            // Arrange
            var sut = new TestObject();
            
            // Act
            sut.FireWhen();
            
            // Assert
            sut.WhenCalled.ShouldBeTrue();
        }
    }
}