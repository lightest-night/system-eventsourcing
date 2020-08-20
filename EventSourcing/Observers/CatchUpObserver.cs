using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
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
        private readonly string _checkpointName;
        private readonly ICheckpointManager _checkpointManager;
        private readonly IReplayManager _replayManager;
        
        private static long? _checkpoint;

        private bool _isActive;

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool IsActive
        {
            get => _isActive;
            private set
            {
                if (value == _isActive)
                    return;

                _isActive = value;
                NotifyPropertyChanged();
            }
        }
        
        public abstract ILogger Logger { get; }

        protected CatchUpObserver(ICheckpointManager checkpointManager, IReplayManager replayManager,
            GetGlobalCheckpoint getGlobalCheckpoint)
        {
            _checkpointName = $"checkpoint-{GetType().Name}";
            _checkpointManager = checkpointManager ?? throw new ArgumentNullException(nameof(checkpointManager));
            _replayManager = replayManager ?? throw new ArgumentNullException(nameof(replayManager));
            
            var globalCheckpoint = (getGlobalCheckpoint ?? throw new ArgumentNullException(nameof(getGlobalCheckpoint)))().Result;
            if (globalCheckpoint == _checkpoint)
                IsActive = true;
            else
                Task.Run(async () => await CatchUp().ConfigureAwait(false));
        }

        public abstract Task EventReceived(object @event, long? position = null, int? version = null,
            CancellationToken cancellationToken = default);

        protected Task SetCheckpoint(long? checkpoint, CancellationToken cancellationToken = default)
        {
            _checkpoint = checkpoint;
            return _checkpoint.HasValue
                ? _checkpointManager.SetCheckpoint(_checkpointName, _checkpoint.Value, cancellationToken)
                : _checkpointManager.ClearCheckpoint(_checkpointName, cancellationToken);
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task CatchUp(CancellationToken cancellationToken = default)
        {
            var projectionName = GetType().FullName;
            var stopwatch = Stopwatch.StartNew();

            var currentCheckpoint = await _replayManager
                .ReplayProjectionFrom(_checkpoint, EventReceived, projectionName, cancellationToken)
                .ConfigureAwait(false);

            await SetCheckpoint(currentCheckpoint, cancellationToken).ConfigureAwait(false);
            IsActive = true;

            stopwatch.Stop();
            Logger.LogInformation($"{projectionName} caught up in {stopwatch.ElapsedMilliseconds}ms");
        }
    }
}