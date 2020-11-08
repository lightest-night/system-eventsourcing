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
// ReSharper disable once OptionalParameterHierarchyMismatch

namespace LightestNight.System.EventSourcing.Tests.Observers
{
    internal class TestCatchUpObserver : CatchUpObserver
    {
        public TestCatchUpObserver(ICheckpointManager checkpointManager, IReplayManager replayManager,
            GetGlobalCheckpoint getGlobalCheckpoint)
            : base(checkpointManager, replayManager, getGlobalCheckpoint)
        { }

        protected override ILogger Logger { get; } = NullLogger.Instance;

        public override Task EventReceived(EventSourceEvent @event, CancellationToken cancellationToken = default)
            => Task.CompletedTask;
    }
    
    public class CatchUpObserverTests
    {
        private const long Checkpoint = 100;
        private readonly Mock<ICheckpointManager> _checkpointManagerMock = new Mock<ICheckpointManager>();
        private readonly Mock<IReplayManager> _replayManagerMock = new Mock<IReplayManager>();
        private readonly CatchUpObserver _sut;

        public CatchUpObserverTests()
        {
            _sut = new TestCatchUpObserver(_checkpointManagerMock.Object, _replayManagerMock.Object,
                cancellationToken => Task.FromResult((long?) Checkpoint));
            
            _checkpointManagerMock
                .Setup(checkpointManager => checkpointManager.GetCheckpoint(_sut.CheckpointName, CancellationToken.None))
                .ReturnsAsync(Checkpoint);
        }
        
        [Fact]
        public async Task ShouldSetActiveToTrueIfCheckpointsAreEqual()
        {
            // Act
            await _sut.InitialiseObserver(CancellationToken.None);
            
            // Allow a pause due to the asynchronous nature of setting checkpoints
            await Task.Delay(50);
            
            // Assert
            _sut.IsActive.ShouldBeTrue();
        }

        [Fact]
        public void ShouldSetActiveToFalseIfCheckpointsAreNotEqual()
        { 
            // Assert
            _sut.IsActive.ShouldBeFalse();
        }
    }
}