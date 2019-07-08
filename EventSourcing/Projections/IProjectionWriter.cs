using System;
using System.Threading.Tasks;

namespace LightestNight.System.EventSourcing.Projections
{
    public interface IProjectionWriter<in TId, TReadModel>
    {
        /// <summary>
        /// Writes a new instance of the Read Model to the projection
        /// </summary>
        /// <param name="item">The item to write</param>
        Task Add(TReadModel item);

        /// <summary>
        /// Writes an update to the instance of the Read Model in the projection using the given predicate
        /// </summary>
        /// <param name="id">The identifier of the Read Model item to update</param>
        /// <param name="update">The predicate that will perform the update</param>
        Task Update(TId id, Action<TReadModel> update);
    }
}