using System.Threading;
using System.Threading.Tasks;

namespace LightestNight.System.EventSourcing.Checkpoints
{
    public interface ICheckpointManager
    {
        /// <summary>
        /// Gets the checkpoint with the given identifier
        /// </summary>
        /// <param name="checkpointId">The checkpoint identifier</param>
        /// <param name="cancellationToken">Any <see cref="CancellationToken" /> used to marshall the operation</param>
        /// <returns>The current checkpoint</returns>
        Task<int?> GetCheckpoint(string checkpointId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets the global checkpoint
        /// </summary>
        /// <param name="cancellationToken">Any <see cref="CancellationToken" /> used to marshall the operation</param>
        /// <returns>The current global checkpoint</returns>
        Task<long?> GetGlobalCheckpoint(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Sets the checkpoint value to the given checkpoint
        /// </summary>
        /// <param name="checkpointId">The checkpoint identifier</param>
        /// <param name="checkpoint">The checkpoint to set</param>
        /// <param name="cancellationToken">Any <see cref="CancellationToken" /> used to marshall the operation</param>
        Task SetCheckpoint<TCheckpoint>(string checkpointId, TCheckpoint checkpoint, CancellationToken cancellationToken = default);
    }
}