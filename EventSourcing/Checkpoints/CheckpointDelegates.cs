using System.Threading;
using System.Threading.Tasks;

namespace LightestNight.EventSourcing.Checkpoints
{
    /// <summary>
    /// Gets the global checkpoint
    /// </summary>
    /// <param name="cancellationToken">Any <see cref="CancellationToken" /> used to marshall the operation</param>
    /// <returns>The current global checkpoint</returns>
    public delegate Task<long?> GetGlobalCheckpoint(CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the global checkpoint value to the given value
    /// </summary>
    /// <param name="checkpoint">The checkpoint to set</param>
    /// <param name="cancellationToken">Any <see cref="CancellationToken" /> used to marshall the operation</param>
    public delegate Task SetGlobalCheckpoint(long? checkpoint, CancellationToken cancellationToken = default);
}