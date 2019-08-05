using System.Threading;
using System.Threading.Tasks;
using LightestNight.System.EventSourcing.Domain;

namespace LightestNight.System.EventSourcing.Observers
{
    public abstract class EventObserver : IEventObserver
    {
        public EventSubscriber Subscriber { get; }

        protected EventObserver()
        {
            Subscriber = new EventSubscriber();
        }

        /// <inheritdoc cref="IEventObserver.GetCurrentPosition" />
        public abstract Task<StreamPosition> GetCurrentPosition(CancellationToken cancellationToken = default);
        
        /// <inheritdoc cref="IEventObserver.SetCurrentPosition" />
        public abstract Task SetCurrentPosition(StreamPosition position, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IEventObserver.GetStatus" />
        public abstract Task<ObserverStatus> GetStatus(CancellationToken cancellationToken = default);

        /// <inheritdoc cref="IEventObserver.SetStatus" />
        public abstract Task SetStatus(ObserverStatus status, CancellationToken cancellationToken = default);
    }
}