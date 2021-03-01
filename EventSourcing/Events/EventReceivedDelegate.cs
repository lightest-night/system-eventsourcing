using System.Threading;
using System.Threading.Tasks;

namespace LightestNight.EventSourcing.Events
{
    /// <summary>
    /// When an Event is received this function is the one that is invoked to process the event.
    /// </summary>
    /// <param name="event">The event that is being observed</param>
    /// <param name="cancellationToken">Any <see cref="CancellationToken" /> in use to marshall the request</param>
    public delegate Task EventReceived(EventSourceEvent @event, CancellationToken cancellationToken = default);
}