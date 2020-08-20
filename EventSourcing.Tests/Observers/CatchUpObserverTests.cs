using System.Threading;
using System.Threading.Tasks;
using LightestNight.System.EventSourcing.Checkpoints;
using LightestNight.System.EventSourcing.Events;
using LightestNight.System.EventSourcing.Observers;
using LightestNight.System.EventSourcing.Replay;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Shouldly;
using Xunit;

namespace LightestNight.System.EventSourcing.Tests.Observers
{
    internal class TestCatchUpObserver : CatchUpObserver
    {
        public TestCatchUpObserver(ICheckpointManager checkpointManager, IReplayManager replayManager,
            GetGlobalCheckpoint getGlobalCheckpoint)
            : base(checkpointManager, replayManager, getGlobalCheckpoint)
        { }

        public override ILogger Logger { get; } = NullLogger.Instance;

        public override Task EventReceived(object @event, long? position = null, int? version = null,
            CancellationToken cancellationToken = default)
            => Task.CompletedTask;
    }
    
    public class CatchUpObserverTests
    {
        private const long Checkpoint = 100;
        private const string CheckpointName = "Test Checkpoint";
        private readonly Mock<ICheckpointManager> _checkpointManagerMock = new Mock<ICheckpointManager>();
        private readonly Mock<IReplayManager> _replayManagerMock = new Mock<IReplayManager>();

        public CatchUpObserverTests()
        {
            _checkpointManagerMock
                .Setup(checkpointManager => checkpointManager.GetCheckpoint(CheckpointName, CancellationToken.None))
                .ReturnsAsync(Checkpoint);
        }
        
        [Fact]
        public async Task ShouldSetActiveToTrueIfCheckpointsAreEqual()
        {
            // Arrange
            _replayManagerMock
                .Setup(replayManager =>
                    replayManager.ReplayProjectionFrom(null, It.IsAny<EventReceived>(), typeof(TestCatchUpObserver).FullName,
                        CancellationToken.None)).ReturnsAsync(Checkpoint);
            
            // Act
            var sut = new TestCatchUpObserver(_checkpointManagerMock.Object, _replayManagerMock.Object,
                cancellationToken => Task.FromResult((long?) Checkpoint));
            
            // Allow a pause due to the asynchronous nature of setting checkpoints
            await Task.Delay(50);
            
            // Assert
            sut.IsActive.ShouldBeTrue();
        }

        [Fact]
        public void ShouldSetActiveToFalseIfCheckpointsAreNotEqual()
        {
            // Act
            var sut = new TestCatchUpObserver(_checkpointManagerMock.Object, _replayManagerMock.Object,
                cancellationToken => Task.FromResult((long?) Checkpoint));
            
           // Assert
           sut.IsActive.ShouldBeFalse();
        }
    }
}