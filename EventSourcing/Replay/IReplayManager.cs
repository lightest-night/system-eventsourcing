using System;
using System.Threading;
using System.Threading.Tasks;

namespace LightestNight.System.EventSourcing.Replay
{
    public interface IReplayManager
    {
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