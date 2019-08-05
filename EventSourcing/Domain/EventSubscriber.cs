namespace LightestNight.System.EventSourcing.Domain
{
    public class EventSubscriber
    {
        /// <summary>
        /// The subscriber to any events
        /// </summary>
        public object Subscriber { get; private set; }

        /// <summary>
        /// Sets the subscriber to the given subscriber object
        /// </summary>
        /// <param name="subscriber">The subscriber to set</param>
        /// <typeparam name="TSubscriber">The type of the subscriber to set</typeparam>
        public void SetSubscriber<TSubscriber>(TSubscriber subscriber)
        {
            Subscriber = subscriber;
        }
    }
}