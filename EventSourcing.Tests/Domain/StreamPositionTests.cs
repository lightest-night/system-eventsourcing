using LightestNight.System.EventSourcing.Domain;
using Shouldly;
using Xunit;

namespace LightestNight.System.EventSourcing.Tests.Domain
{
    public class StreamPositionTests
    {
        [Theory]
        [InlineData(10, 25)]
        [InlineData(15, 20)]
        [InlineData(15, 25)]
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
        [InlineData(10, 10)]
        [InlineData(9, 25)]
        [InlineData(5, 5)]
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
        [InlineData(50, 70)]
        [InlineData(40, 80)]
        [InlineData(40, 70)]
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
        [InlineData(60, 80)]
        [InlineData(50, 77)]
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