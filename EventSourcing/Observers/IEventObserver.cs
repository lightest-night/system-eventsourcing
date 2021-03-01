using System.Threading;
using System.Threading.Tasks;
using LightestNight.EventSourcing.Events;

namespace LightestNight.EventSourcing.Observers
{
    public interface IEventObserver
    {
        /// <summary>
        /// Called by our runtime to initialise the observer and get it ready to work
        /// </summary>
        /// <param name="cancellationToken">Any <see cref="CancellationToken" /> in use to marshall the request</param>
        Task InitialiseObserver(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// When an Event is received this function is the one that is invoked to process the event.
        /// </summary>
        /// <param name="evt">The event that is being observed</param>
        /// <param name="cancellationToken">Any <see cref="CancellationToken" /> in use to marshall the request</param>
        Task EventReceived(EventSourceEvent evt, CancellationToken cancellationToken = default);

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