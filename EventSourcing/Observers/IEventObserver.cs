using LightestNight.System.EventSourcing.Domain;

namespace LightestNight.System.EventSourcing.Observers
{
    /// <summary>
    /// An interface to mark a class as an event observer
    /// </summary>
    public interface IEventObserver
    {
        /// <summary>
        /// Gets the current position that this Observer is at
        /// </summary>
        /// <returns>An instance of <see cref="StreamPosition" /></returns>
        StreamPosition GetCurrentPosition();

        /// <summary>
        /// Sets the current position this Observer is at
        /// </summary>
        /// <param name="position">The <see cref="StreamPosition" /> to set</param>
        void SetCurrentPosition(StreamPosition position);
    }
}