namespace LightestNight.System.EventSourcing.Domain
{
    public class StreamPosition
    {
        /// <summary>
        /// Commit position of the Event
        /// </summary>
        public long Commit { get; set; }
        
        /// <summary>
        /// Prepare position of the Event
        /// </summary>
        public long Prepare { get; set; }
    }
}