using System.Threading;
using System.Threading.Tasks;
using LightestNight.System.EventSourcing.Events;
using NotImplementedException = System.NotImplementedException;

namespace LightestNight.System.EventSourcing.Tests.Observers
{
    public class TestObserver : IEventObserver
    {
        public ValueTask DisposeAsync()
        {
            throw new NotImplementedException();
        }
        
        public void Dispose(){}

        public bool IsActive { get; }
        public bool IsDisposed { get; set; }

        public Task InitialiseObserver(CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        public Task EventReceived(object evt, long? position = default, int? version = default,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}