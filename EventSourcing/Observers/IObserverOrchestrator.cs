using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace LightestNight.System.EventSourcing.Observers
{
    public interface IObserverOrchestrator<in TEvent, in TSubscription> : IHostedService
    {
        /// <summary>
        /// When a subscription event materializes, this function will be called to dispatch it properly
        /// </summary>
        /// <param name="subscription">The subscription that raised the event</param>
        /// <param name="event">The event the subscription has raised</param>
        Task OnEventMaterialized(TSubscription subscription, TEvent @event);

        /// <summary>
        /// When all previous events have been processed, this function will be called
        /// </summary>
        /// <remarks>
        /// Not all implementations of this interface will use this function. Only implementations that utilise catch up subscriptions.
        /// GetEventStore is one such example
        /// This is normally just used to log that catch up has finished
        /// </remarks>
        /// <param name="subscription">The subscription that is being observed</param>
        void OnLiveProcessingStarted(TSubscription subscription);

        /// <summary>
        /// When the subscription is dropped, this function will be called
        /// </summary>
        /// <param name="subscription">The subscription that is being observed, but has now been dropped</param>
        /// <param name="dropReason">Any reason for the subscription drop</param>
        /// <param name="ex">Any <see cref="Exception" /> that was raised when the subscription dropped</param>
        /// <typeparam name="TDropReason">Type of the reason this subscription dropped</typeparam>
        void OnSubscriptionDropped<TDropReason>(TSubscription subscription, TDropReason dropReason, Exception ex);
    }
}