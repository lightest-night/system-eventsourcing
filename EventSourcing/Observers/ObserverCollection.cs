using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LightestNight.System.EventSourcing.Events;

namespace LightestNight.System.EventSourcing.Observers
{
    public static class ObserverCollection
    {
        private static readonly ConcurrentDictionary<string, IEventObserver> Observers = new ConcurrentDictionary<string, IEventObserver>();

        public delegate void EventObserverEventHandler(EventObserverEventArgs e);

        public static event EventObserverEventHandler? EventObserverRegistered;
        public static event EventObserverEventHandler? EventObserverUnregistered;

        /// <summary>
        /// Registers an <see cref="IEventObserver" /> into the application
        /// </summary>
        /// <param name="observer">The observer to register</param>
        /// <param name="cancellationToken">Any <see cref="CancellationToken" /> in use to marshall the request</param>
        /// <exception cref="InvalidOperationException">If the Observer cannot be registered</exception>
        public static async Task RegisterObserverAsync(IEventObserver observer, CancellationToken cancellationToken = default)
        {
            await observer.InitialiseObserver(cancellationToken).ConfigureAwait(false);
            var observerName = observer.GetObserverName();
            if (!Observers.TryAdd(observerName, observer))
                throw new InvalidOperationException($"Could not add {observerName} to the Observers Collection");
            
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
            Observers.Remove(observer.GetObserverName(), out _);
            
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