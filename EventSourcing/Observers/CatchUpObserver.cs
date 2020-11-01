using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using LightestNight.System.EventSourcing.Checkpoints;
using LightestNight.System.EventSourcing.Events;
using LightestNight.System.EventSourcing.Replay;
using Microsoft.Extensions.Logging;

namespace LightestNight.System.EventSourcing.Observers
{
    public abstract class CatchUpObserver : IEventObserver
    {
        private readonly ICheckpointManager _checkpointManager;
        private readonly IReplayManager _replayManager;
        private readonly GetGlobalCheckpoint _getGlobalCheckpoint;

        private static long? _checkpoint;
        
        /// <inheritdoc cref="IEventObserver.IsActive" />
        public virtual bool IsActive { get; set; }
        
        /// <inheritdoc cref="IEventObserver.IsReplaying" />
        public virtual bool IsReplaying { get; set; }
        
        public string CheckpointName => $"checkpoint-{GetType().FullName}";
        
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

        /// <inheritdoc cref="IEventObserver.EventReceived" />
        public abstract Task EventReceived(EventSourceEvent @event, long? position = default, int? version = default,
            CancellationToken cancellationToken = default);

        public async Task InitialiseObserver(CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"{GetType().FullName}.{nameof(InitialiseObserver)} executing...");

            _checkpoint = await _checkpointManager.GetCheckpoint(CheckpointName, cancellationToken)
                .ConfigureAwait(false);
            
            var globalCheckpoint = await _getGlobalCheckpoint(cancellationToken).ConfigureAwait(false);
            if (globalCheckpoint == _checkpoint)
                IsActive = true;
            else await CatchUp(cancellationToken).ConfigureAwait(false);
        }
        
        protected Task SetCheckpoint(long? checkpoint, CancellationToken cancellationToken = default)
        {
            _checkpoint = checkpoint;

            return _checkpoint.HasValue
                ? _checkpointManager.SetCheckpoint(CheckpointName, _checkpoint.Value, cancellationToken)
                : _checkpointManager.ClearCheckpoint(CheckpointName, cancellationToken);
        }

        private async Task CatchUp(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            var projectionName = GetType().FullName;
            var stopwatch = Stopwatch.StartNew();

            IsReplaying = true;
            var currentCheckpoint = await _replayManager
                .ReplayProjectionFrom(_checkpoint, EventReceived, projectionName, cancellationToken)
                .ConfigureAwait(false);
            await SetCheckpoint(currentCheckpoint, cancellationToken).ConfigureAwait(false);

            IsReplaying = false;
            IsActive = true;

            stopwatch.Stop();
            Logger.LogInformation($"{projectionName} caught up in {stopwatch.ElapsedMilliseconds}ms");
        }
    }
}