using System;
using LightestNight.System.EventSourcing.Domain;

namespace LightestNight.System.EventSourcing
{
    public class AggregateDeletedException : Exception
    {
        public AggregateDeletedException(IEventSourceAggregate aggregate) : base(
            $"Aggregate {aggregate.GetType().Name} with identifier {aggregate.Id} was in a deleted state")
        {
        }

        public AggregateDeletedException(IEventSourceAggregate aggregate, Exception inner) : base(
            $"Aggregate {aggregate.GetType().Name} with identifier {aggregate.Id} was in a deleted state", inner)
        {
        }
    }
}