using LightestNight.System.EventSourcing.Domain;
using Shouldly;
using Xunit;

namespace LightestNight.System.EventSourcing.Tests.Domain
{
    public class StreamPositionTests
    {
        [Theory]
        [InlineData(-1, -1)]
        [InlineData(10, 15)]
        [InlineData(5, 20)]
        public void Should_Return_True_When_Greater(long commit, long prepare)
        {
            // Arrange
            var sut = new StreamPosition(10, 20);
            
            // Act
            var result = sut > new StreamPosition(commit, prepare);
            
            // Assert
            result.ShouldBeTrue();
        }

        [Theory]
        [InlineData(10, 30)]
        [InlineData(15, 20)]
        [InlineData(10, 20)]
        public void Should_Return_False_When_Not_Greater(long commit, long prepare)
        {
            // Arrange
            var sut = new StreamPosition(10, 20);
            
            // Act
            var result = sut > new StreamPosition(commit, prepare);
            
            // Assert
            result.ShouldBeFalse();
        }

        [Theory]
        [InlineData(50, 80)]
        [InlineData(65, 75)]
        [InlineData(65, 85)]
        public void Should_Return_True_When_Lower(long commit, long prepare)
        {
            // Arrange
            var sut = new StreamPosition(50, 75);
            
            // Act
            var result = sut < new StreamPosition(commit, prepare);
            
            // Assert
            result.ShouldBeTrue();
        }
        
        [Theory]
        [InlineData(50, 75)]
        [InlineData(45, 75)]
        [InlineData(50, 60)]
        public void Should_Return_False_When_Not_Lower(long commit, long prepare)
        {
            // Arrange
            var sut = new StreamPosition(50, 75);
            
            // Act
            var result = sut < new StreamPosition(commit, prepare);
            
            // Assert
            result.ShouldBeFalse();
        }

        [Fact]
        public void Should_Return_True_When_Equal()
        {
            // Arrange
            var sut = new StreamPosition(50, 75);
            
            // Act
            var result = sut == new StreamPosition(50, 75);
            
            // Assert
            result.ShouldBeTrue();
        }
    }
}