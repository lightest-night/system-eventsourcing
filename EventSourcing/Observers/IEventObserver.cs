using System.Threading;
using System.Threading.Tasks;
using LightestNight.System.EventSourcing.Domain;

namespace LightestNight.System.EventSourcing.Observers
{
    /// <summary>
    /// An interface to mark a class as an event observer
    /// </summary>
    public interface IEventObserver
    {
        /// <summary>
        /// Gets the current position that this Observer is at
        /// </summary>
        /// <param name="cancellationToken">Any <see cref="CancellationToken" /> to use during the request</param>
        /// <returns>An instance of <see cref="StreamPosition" /></returns>
        Task<StreamPosition> GetCurrentPosition(CancellationToken cancellationToken = default);

        /// <summary>
        /// Sets the current position this Observer is at
        /// </summary>
        /// <param name="position">The <see cref="StreamPosition" /> to set</param>
        /// <param name="cancellationToken">Any <see cref="CancellationToken" /> to use during the request</param>
        Task SetCurrentPosition(StreamPosition position, CancellationToken cancellationToken);
    }
}