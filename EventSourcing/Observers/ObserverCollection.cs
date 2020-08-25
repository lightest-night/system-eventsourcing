using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LightestNight.System.EventSourcing.Events;

namespace LightestNight.System.EventSourcing.Observers
{
    public static class ObserverCollection
    {
        private static readonly Dictionary<string, IEventObserver> Observers = new Dictionary<string, IEventObserver>();

        /// <summary>
        /// Registers an <see cref="IEventObserver" /> into the application
        /// </summary>
        /// <param name="observer">The observer to register</param>
        /// <param name="cancellationToken">Any <see cref="CancellationToken" /> in use to marshall the request</param>
        public static async Task RegisterObserver(IEventObserver observer, CancellationToken cancellationToken = default)
        {
            await observer.InitialiseObserver(cancellationToken).ConfigureAwait(false);
            Observers.Add(observer.GetType().FullName, observer);
        }

        /// <summary>
        /// Unregisters an <see cref="IEventObserver" /> from the application
        /// </summary>
        /// <param name="observer">The observer to unregister</param>
        public static async Task UnregisterObserver(IEventObserver observer)
        {
            if (!observer.IsDisposed)
            {
                await observer.DisposeAsync();
                observer.IsDisposed = true;
            }
        }

        /// <summary>
        /// Gets a collection of <see cref="IEventObserver" /> instances that have been registered into the application
        /// </summary>
        /// <returns>Collection of <see cref="IEventObserver" /> instances</returns>
        public static IEnumerable<IEventObserver> GetEventObservers()
            => Observers.Values;
    }
}