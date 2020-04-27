namespace LightestNight.System.EventSourcing
{
    public class EventSourcingOptions
    {
        /// <summary>
        /// The amount of times a subscription will attempt to reconnect before reporting failure
        /// </summary>
        public int SubscriptionRetryCount { get; set; } = 5;

        /// <summary>
        /// When reading an Event Store Stream forward, the maximum amount of events to retrieve per page
        /// </summary>
        public int MaxReadStreamForward { get; set; } = 500;

        /// <summary>
        /// The amount of events that a subscription needs to be behind before we run a fast forward rather than a subscription
        /// </summary>
        public int SubscriptionCheckpointDelta { get; set; } = 50;
    }
}