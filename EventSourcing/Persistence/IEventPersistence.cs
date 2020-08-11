using System.Threading;
using System.Threading.Tasks;
using LightestNight.System.EventSourcing.Domain;

namespace LightestNight.System.EventSourcing.Persistence
{
    public interface IEventPersistence
    {
        /// <summary>
        /// Gets an <see cref="IEventSourceAggregate" /> object from the persistence store by it's identifier
        /// </summary>
        /// <param name="id">The Globally Unique Identifier of the <see cref="IEventSourceAggregate" /> object</param>
        /// <param name="cancellationToken">Any <see cref="CancellationToken" /> needed to marshal the operation</param>
        /// <typeparam name="TAggregate">The type of the <see cref="IEventSourceAggregate" /> to retrieve</typeparam>
        /// <returns>A populated instance of <see cref="IEventSourceAggregate" /></returns>
        Task<TAggregate> GetById<TAggregate>(object id, CancellationToken cancellationToken = default)
            where TAggregate : class, IEventSourceAggregate;

        /// <summary>
        /// Writes the given <see cref="IEventSourceAggregate" /> object to the persistence store
        /// </summary>
        /// <param name="aggregate">The <see cref="IEventSourceAggregate" /> to write</param>
        /// <param name="cancellationToken">Any <see cref="CancellationToken" /> needed to marshal the operation</param>
        Task Save(IEventSourceAggregate aggregate, CancellationToken cancellationToken = default);
    }
}