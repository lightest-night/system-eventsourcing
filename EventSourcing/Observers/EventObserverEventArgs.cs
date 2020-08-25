using System;
using LightestNight.System.EventSourcing.Events;

namespace LightestNight.System.EventSourcing.Observers
{
    public class EventObserverEventArgs : EventArgs
    {
        /// <summary>
        /// The <see cref="IEventObserver" /> instance that was registered into the application
        /// </summary>
        public IEventObserver EventObserver { get; set; } = default!;
    }
}