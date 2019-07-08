using System.Threading.Tasks;

namespace LightestNight.System.EventSourcing.Observers.Rules
{
    public interface IEventRule
    {
        /// <summary>
        /// Takes in an <see cref="SubscriptionEvent" /> object and returns a boolean about whether this event should be processed
        /// </summary>
        /// <param name="event">The event object to test</param>
        /// <returns>Boolean denoting whether the event should be processed</returns>
        Task<bool> Execute(SubscriptionEvent @event);
    }
}