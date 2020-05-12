using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace LightestNight.System.EventSourcing.Replay
{
    public interface IReplayManager
    {
        /// <summary>
        /// Replays the global stream firing the <paramref name="eventReceived" /> function for each
        /// </summary>
        /// <param name="fromCheckpoint">The current checkpoint</param>
        /// <param name="eventReceived">Function to invoke when an event is received</param>
        /// <param name="projectionName">The name of the projection that is being replayed</param>
        /// <param name="cancellationToken">Any <see cref="CancellationToken" /> used to marshall the operation</param>
        /// <returns>The value of where in the stream the new checkpoint is</returns>
        Task<long> ReplayProjectionFrom(long? fromCheckpoint, Func<object, CancellationToken, Task> eventReceived, [CallerMemberName] string? projectionName = default, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Replays a stream of events firing the <paramref name="eventReceived" /> function for each
        /// </summary>
        /// <param name="streamId">The identifier of the stream to catch up</param>
        /// <param name="fromCheckpoint">The current checkpoint</param>
        /// <param name="eventReceived">Function to invoke when an event is received</param>
        /// <param name="cancellationToken">Any <see cref="CancellationToken" /> used to marshall the operation</param>
        /// <returns>The value of where in the stream the new checkpoint is</returns>
        Task<int> ReplayProjectionFrom(string streamId, int fromCheckpoint, Func<object, CancellationToken, Task> eventReceived, CancellationToken cancellationToken = default);
    }
}