namespace LightestNight.System.EventSourcing.Projections
{
    public interface IProjectionWriterFactory
    {
        /// <summary>
        /// Factory method to build a new instance of an <see cref="IProjectionWriter{TId,TReadModel}" />
        /// </summary>
        /// <typeparam name="TId">The type of the Read Model item's identifier</typeparam>
        /// <typeparam name="TReadModel">The type of the Read Model item</typeparam>
        IProjectionWriter<TId, TReadModel> GetProjectionWriter<TId, TReadModel>()
            where TReadModel : EventSourceReadModel<TId>;
    }
}