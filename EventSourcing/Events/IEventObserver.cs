using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace LightestNight.System.EventSourcing.Events
{
    public interface IEventObserver : INotifyPropertyChanged
    {
        /// <summary>
        /// When an Event is received this function is the one that is invoked to process the event.
        /// </summary>
        /// <param name="event">The event that is being observed</param>
        /// <param name="position">The position in the stream the received event occurred</param>
        /// <param name="version">The version of the received event</param> 
        /// <param name="cancellationToken">Any <see cref="CancellationToken" /> in use to marshall the request</param>
        /// <typeparam name="TEvent">The type of the event being observed</typeparam>
        Task EventReceived<TEvent>(TEvent @event, long? position = default, int? version = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// Denotes whether the observer is active, if not, any events received will be ignored
        /// </summary>
        bool IsActive { get; }
    }
}