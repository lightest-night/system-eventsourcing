namespace LightestNight.System.EventSourcing.Observers.Rules.Mappers
{
    public interface IEventRuleMapper<in TOriginalSubscriptionEvent>
    {
        /// <summary>
        /// Takes an instance of <typeparamref name="TOriginalSubscriptionEvent" /> and maps it to an instance of <see cref="SubscriptionEvent "/>
        /// </summary>
        /// <param name="originalSubscriptionEvent">The original subscription event to map from</param>
        /// <returns>A populated instance of <see cref="SubscriptionEvent" /></returns>
        SubscriptionEvent Map(TOriginalSubscriptionEvent originalSubscriptionEvent);
    }
}