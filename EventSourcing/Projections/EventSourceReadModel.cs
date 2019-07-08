namespace LightestNight.System.EventSourcing.Projections
{
    public class EventSourceReadModel<TId>
    {
        /// <summary>
        /// The Globally Unique Identifier of this read model
        /// </summary>
        public TId Id { get; set; }
    }
}