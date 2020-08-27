using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LightestNight.System.EventSourcing.Events;

namespace LightestNight.System.EventSourcing.Observers
{
    public static class ObserverCollection
    {
        private static readonly Dictionary<string, IEventObserver> Observers = new Dictionary<string, IEventObserver>();

        public delegate void EventObserverEventHandler(EventObserverEventArgs e);

        public static event EventObserverEventHandler? EventObserverRegistered;
        public static event EventObserverEventHandler? EventObserverUnregistered;

        /// <summary>
        /// Registers an <see cref="IEventObserver" /> into the application
        /// </summary>
        /// <param name="observer">The observer to register</param>
        /// <param name="cancellationToken">Any <see cref="CancellationToken" /> in use to marshall the request</param>
        public static async Task RegisterObserverAsync(IEventObserver observer, CancellationToken cancellationToken = default)
        {
            await observer.InitialiseObserver(cancellationToken).ConfigureAwait(false);
            Observers.Add(observer.GetType().FullName, observer);
            
            EventObserverRegistered?.Invoke(new EventObserverEventArgs
            {
                EventObserver = observer
            });
        }

        /// <summary>
        /// Unregisters an <see cref="IEventObserver" /> from the application
        /// </summary>
        /// <param name="observer">The observer to unregister</param>
        public static void UnregisterObserver(IEventObserver observer)
        {
            Observers.Remove(observer.GetType().FullName);
            
            EventObserverUnregistered?.Invoke(new EventObserverEventArgs
            {
                EventObserver = observer
            });
        }

        /// <summary>
        /// Gets a collection of <see cref="IEventObserver" /> instances that have been registered into the application
        /// </summary>
        /// <returns>Collection of <see cref="IEventObserver" /> instances</returns>
        public static IEnumerable<IEventObserver> GetEventObservers()
            => Observers.Values;
    }
}