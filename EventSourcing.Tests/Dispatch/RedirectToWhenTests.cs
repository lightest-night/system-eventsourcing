using LightestNight.EventSourcing.Dispatch;
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

            private void When(TestEvent evt)
            {
                WhenCalled = true;
            }
            
            private static void When(WrongTestEvent evt){}
        }

        private class TestEvent{}

        private class WrongTestEvent{}

        [Fact]
        public void ShouldDispatchToCorrectEvent()
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