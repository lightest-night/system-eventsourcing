using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using LightestNight.EventSourcing.Checkpoints;
using LightestNight.EventSourcing.Events;
using LightestNight.EventSourcing.Replay;
using Microsoft.Extensions.Logging;

namespace LightestNight.EventSourcing.Observers
{
    public abstract class CatchUpObserver : IEventObserver
    {
        private readonly ICheckpointManager _checkpointManager;
        private readonly IReplayManager _replayManager;
        private readonly GetGlobalCheckpoint _getGlobalCheckpoint;

        private static long? _checkpoint;

        /// <summary>
        /// The name of the checkpoint this Observer uses
        /// </summary>
        public string CheckpointName => $"checkpoint-{GetType().FullName}";
        
        /// <summary>
        /// The current value of this Observer's checkpoint
        /// </summary>
        public long? Checkpoint => _checkpoint;
        
        /// <summary>
        /// <see cref="ILogger" /> instance to output meaningful log messages
        /// </summary>
        protected abstract ILogger Logger { get; }

        protected CatchUpObserver(ICheckpointManager checkpointManager, IReplayManager replayManager,
            GetGlobalCheckpoint getGlobalCheckpoint)
        {
            _checkpointManager = checkpointManager ?? throw new ArgumentNullException(nameof(checkpointManager));
            _replayManager = replayManager ?? throw new ArgumentNullException(nameof(replayManager));
            _getGlobalCheckpoint = getGlobalCheckpoint ?? throw new ArgumentNullException(nameof(getGlobalCheckpoint));
        }

        /// <summary>
        /// Processes the <see cref="EventSourceEvent" />
        /// </summary>
        /// <param name="event">The <see cref="EventSourceEvent" /> to process</param>
        /// <param name="cancellationToken">Any <see cref="CancellationToken" /> used to marshall the operation</param>
        public abstract Task ProcessEvent(EventSourceEvent @event, CancellationToken cancellationToken);

        /// <inheritdoc cref="IEventObserver.InitialiseObserver" />
        public async Task InitialiseObserver(CancellationToken cancellationToken = default)
        {
            Logger.LogInformation("{observerType} is initialising...", GetType().FullName);

            _checkpoint = await _checkpointManager.GetCheckpoint(CheckpointName, cancellationToken)
                .ConfigureAwait(false);
            var globalCheckpoint = await _getGlobalCheckpoint(cancellationToken).ConfigureAwait(false);

            if (_checkpoint != globalCheckpoint)
                await CatchUp(cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc cref="IEventObserver.EventReceived" />
        public virtual async Task EventReceived(EventSourceEvent @event, CancellationToken cancellationToken = default)
        {
            Logger.LogDebug("Projector {projector} processing event: {eventType}", GetType().Name,
                @event.GetType().Name);

            await ProcessEvent(@event, cancellationToken).ConfigureAwait(false);
            await SetCheckpoint(@event.Position, cancellationToken).ConfigureAwait(false);
        }

        protected Task SetCheckpoint(long? checkpoint, CancellationToken cancellationToken)
        {
            _checkpoint = checkpoint;

            return _checkpoint.HasValue
                ? _checkpointManager.SetCheckpoint(CheckpointName, _checkpoint.Value, cancellationToken)
                : _checkpointManager.ClearCheckpoint(CheckpointName, cancellationToken);
        }

        private async Task CatchUp(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            var projectionName = GetType().FullName;
            var stopwatch = Stopwatch.StartNew();

            var newCheckpoint = await _replayManager
                .ReplayProjectionFrom(_checkpoint, ProcessEvent, projectionName, cancellationToken)
                .ConfigureAwait(false);
            await SetCheckpoint(newCheckpoint, cancellationToken).ConfigureAwait(false);

            stopwatch.Stop();
            Logger.LogInformation("{projectionName} caught up in {ms}ms", projectionName, stopwatch.ElapsedMilliseconds);
        }
    }
}