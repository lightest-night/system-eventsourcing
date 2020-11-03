using System.Threading;
using System.Threading.Tasks;

namespace LightestNight.System.EventSourcing.Events
{
    public interface IEventObserver
    {
        /// <summary>
        /// Denotes whether the observer is active, if not, any events received will be ignored
        /// </summary>
        bool IsActive { get; }
        
        /// <summary>
        /// Denotes whether the observer is currently replaying old events
        /// </summary>
        bool IsReplaying { get; }
        
        /// <summary>
        /// Called by our runtime to initialise the observer and get it ready to work
        /// </summary>
        /// <param name="cancellationToken">Any <see cref="CancellationToken" /> in use to marshall the request</param>
        Task InitialiseObserver(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// When an Event is received this function is the one that is invoked to process the event.
        /// </summary>
        /// <param name="evt">The event that is being observed</param>
        /// <param name="position">The position in the stream the received event occurred</param>
        /// <param name="version">The version of the received event</param> 
        /// <param name="cancellationToken">Any <see cref="CancellationToken" /> in use to marshall the request</param>
        Task EventReceived(EventSourceEvent evt, long? position = default, int? version = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the Observer's name
        /// </summary>
        /// <returns>The name of the Observer</returns>
        public string GetObserverName()
        {
            var observerType = GetType();
            return observerType.FullName ?? observerType.GetGenericTypeDefinition().FullName!;
        }
    }
}