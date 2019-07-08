namespace LightestNight.System.EventSourcing.Observers.Rules
{
    public class SubscriptionEvent
    {
        /// <summary>
        /// The type of the original event that triggered the subscription
        /// </summary>
        public string EventType { get; set; }
        
        /// <summary>
        /// Any metadata associated with the event that triggered the subscription
        /// </summary>
        public byte[] Metadata { get; set; }
    }
}