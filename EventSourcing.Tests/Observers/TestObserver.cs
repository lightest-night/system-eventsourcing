using System.Threading;
using System.Threading.Tasks;
using LightestNight.System.EventSourcing.Events;
using NotImplementedException = System.NotImplementedException;

namespace LightestNight.System.EventSourcing.Tests.Observers
{
    public class TestObserver : IEventObserver
    {
        public bool IsActive { get; }
        public bool IsReplaying { get; set; }

        public Task InitialiseObserver(CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        public Task EventReceived(EventSourceEvent evt, long? position = default, int? version = default,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}