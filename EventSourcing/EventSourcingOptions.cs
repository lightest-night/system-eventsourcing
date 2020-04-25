namespace LightestNight.System.EventSourcing
{
    public class EventSourcingOptions
    {
        /// <summary>
        /// The amount of times a subscription will attempt to reconnect before reporting failure
        /// </summary>
        public int SubscriptionRetryCount { get; set; } = 5;
    }
}