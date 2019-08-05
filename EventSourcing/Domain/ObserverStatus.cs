namespace LightestNight.System.EventSourcing.Domain
{
    public class ObserverStatus
    {
        /// <summary>
        /// The name of the Observer
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The name of the Event Stream the Observer is subscribed to
        /// </summary>
        public string StreamName { get; set; }
        
        /// <summary>
        /// The current position the observer is at
        /// </summary>
        public long Position { get; set; }
        
        /// <summary>
        /// The number of events to read at one time
        /// </summary>
        public int BufferSize { get; set; }
    }
}