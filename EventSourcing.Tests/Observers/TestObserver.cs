using System.Threading;
using System.Threading.Tasks;
using LightestNight.System.EventSourcing.Events;
using LightestNight.System.EventSourcing.Observers;
using NotImplementedException = System.NotImplementedException;

namespace LightestNight.System.EventSourcing.Tests.Observers
{
    public class TestObserver : IEventObserver
    {
        private readonly int? _index;
        
        public TestObserver(){}
        
        public TestObserver(int index)
        {
            _index = index;
        }
        
        public bool IsActive { get; }
        public bool IsReplaying { get; set; }

        public Task InitialiseObserver(CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        public Task EventReceived(EventSourceEvent evt, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public string GetObserverName()
            => $"{nameof(TestObserver)}-{_index}";
    }
}