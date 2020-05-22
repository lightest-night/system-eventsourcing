using System.Threading;
using System.Threading.Tasks;

namespace LightestNight.System.EventSourcing.Checkpoints
{
    public interface ICheckpointManager
    {
        /// <summary>
        /// Sets a given checkpoint
        /// </summary>
        /// <param name="checkpointName">If set, a name to assign the checkpoint (defaults to the calling member's name)</param>
        /// <param name="checkpoint">The checkpoint to set</param>
        /// <param name="cancellationToken">Any <see cref="CancellationToken" /> used to marshall the operation</param>
        Task SetCheckpoint(string checkpointName, long checkpoint, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the checkpoint with the given name
        /// </summary>
        /// <param name="checkpointName">The checkpoint name (defaults to the calling member's name)</param>
        /// <param name="cancellationToken">Any <see cref="CancellationToken" /> used to marshall the operation</param>
        /// <returns>The checkpoint</returns>
        Task<long?> GetCheckpoint(string checkpointName, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Clears the checkpoint with the given name
        /// </summary>
        /// <param name="checkpointName">The checkpoint name (defaults to the calling member's name)</param>
        /// <param name="cancellationToken">Any <see cref="CancellationToken" /> used to marshall the operation</param>
        Task ClearCheckpoint(string checkpointName, CancellationToken cancellationToken = default);
    }
}