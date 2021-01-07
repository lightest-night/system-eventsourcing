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

        public override Task ProcessEvent(EventSourceEvent @event, CancellationToken cancellationToken = default)
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
                _ => Task.FromResult((long?) Checkpoint));
            
            _replayManagerMock.Setup(replayManager =>
                    replayManager.ReplayProjectionFrom(It.IsAny<long?>(), _sut.ProcessEvent, It.IsAny<string>(),
                        CancellationToken.None))
                .ReturnsAsync(Checkpoint);
        }

        [Fact]
        public async Task Should_Ask_For_Replay_If_Checkpoints_Are_Not_Equal()
        {
            // Arrange
            _checkpointManagerMock
                .Setup(checkpointManager => checkpointManager.GetCheckpoint(_sut.CheckpointName, CancellationToken.None))
                .ReturnsAsync(Checkpoint - 50);
            
            // Act
            await _sut.InitialiseObserver(CancellationToken.None);
            
            // Assert
            _sut.Checkpoint.ShouldBe(Checkpoint);
        }
    }
}