using System;
using System.Runtime.CompilerServices;
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

        /// <summary>
        /// Saves the given checkpoint under the given name
        /// </summary>
        /// <param name="checkpoint">The checkpoint to save</param>
        /// <param name="checkpointName">The name of the checkpoint</param>
        /// <param name="cancellationToken">Any <see cref="CancellationToken" /> used to marshall the operation</param>
        Task SaveCheckpoint(int checkpoint, [CallerMemberName] string? checkpointName = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// Saves the global checkpoint
        /// </summary>
        /// <param name="checkpoint">The global checkpoint to set</param>
        Task SaveGlobalCheckpoint(int checkpoint);

        /// <summary>
        /// The current global checkpoint position
        /// </summary>
        int GlobalCheckpoint { get; }
    }
}