using System.Threading;
using System.Threading.Tasks;
using LightestNight.System.EventSourcing.Domain;

namespace LightestNight.System.EventSourcing.Persistence
{
    public interface IEventPersistence
    {
        /// <summary>
        /// Gets an <see cref="IEventSourceAggregate{TId}" /> object from the persistence store by it's identifier
        /// </summary>
        /// <param name="id">The Globally Unique Identifier of the <see cref="IEventSourceAggregate{TId}" /> object</param>
        /// <param name="cancellationToken">Any <see cref="CancellationToken" /> needed to marshal the operation</param>
        /// <typeparam name="TId">The type of the identifier of the <see cref="IEventSourceAggregate{TId}" /></typeparam>
        /// <typeparam name="TAggregate">The type of the <see cref="IEventSourceAggregate{TId}" /> to retrieve</typeparam>
        /// <returns>A populated instance of <see cref="IEventSourceAggregate{TId}" /></returns>
        Task<TAggregate> GetById<TId, TAggregate>(TId id, CancellationToken cancellationToken = default)
            where TAggregate : class, IEventSourceAggregate<TId>;

        /// <summary>
        /// Writes the given <see cref="IEventSourceAggregate{TId}" /> object to the persistence store
        /// </summary>
        /// <param name="aggregate">The <see cref="IEventSourceAggregate{TId}" /> to write</param>
        /// <param name="cancellationToken">Any <see cref="CancellationToken" /> needed to marshal the operation</param>
        Task Save<TId>(IEventSourceAggregate<TId> aggregate, CancellationToken cancellationToken = default);
    }
}