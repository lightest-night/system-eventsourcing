using System;
using System.Text.Json;

namespace LightestNight.System.EventSourcing.Events
{
    public abstract class EventSourceEvent
    {
        /// <summary>
        /// Allows an event to contain a Correlation Id in order to link it to other events
        /// </summary>
        protected virtual Guid CorrelationId { get; } = Guid.NewGuid();
        
        /// <summary>
        /// The position in the stream this Event takes up
        /// </summary>
        public long? Position { get; set; }
        
        public virtual byte[] Serialize(Type eventType, JsonSerializerOptions? options = null)
            => JsonSerializer.SerializeToUtf8Bytes(this, eventType, options);
        
        public virtual object? Deserialize(byte[] value, Type eventType, JsonSerializerOptions? options = null)
            => JsonSerializer.Deserialize(value, eventType, options);
    }
}