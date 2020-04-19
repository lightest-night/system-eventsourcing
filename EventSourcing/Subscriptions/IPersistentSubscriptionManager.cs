using System;
using System.Threading;
using System.Threading.Tasks;

namespace LightestNight.System.EventSourcing.Subscriptions
{
    public interface IPersistentSubscriptionManager
    {
        /// <summary>
        /// Creates a subscription to the given category
        /// </summary>
        /// <param name="categoryName">The name of the category to subscribe to</param>
        /// <param name="eventReceived">Function to invoke when an event is received</param>
        /// <param name="cancellationToken">Any <see cref="CancellationToken" /> used to marshall the operation</param>
        /// <returns>The globally unique identifier of the subscription</returns>
        Task<Guid> CreateCategorySubscription(string categoryName, Func<object, CancellationToken, Task> eventReceived, CancellationToken cancellationToken = default);

        /// <summary>
        /// Closes the subscription with the given identifier
        /// </summary>
        /// <param name="subscriptionId">The globally unique identifier of the subscription</param>
        /// <param name="cancellationToken">Any <see cref="CancellationToken" /> used to marshall the operation</param>
        Task CloseSubscription(Guid subscriptionId, CancellationToken cancellationToken = default);

    }
}