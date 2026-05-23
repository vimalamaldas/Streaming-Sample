using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace StreamingSample.API.Services
{
    /// <summary>
    /// Simulates a streaming SDK client that produces data incrementally.
    /// </summary>
    /// <remarks>
    /// In a real integration, this class would wrap your SDK's streaming API and
    /// translate its data into an <see cref="IAsyncEnumerable{T}"/> sequence.
    /// </remarks>
    public class StreamingSdkClient : IStreamingSdk
    {
        /// <summary>
        /// Returns an async stream of text chunks.
        /// </summary>
        /// <param name="cancellationToken">Cancels the stream if the HTTP request disconnects.</param>
        /// <returns>An async enumerable that yields messages as they become available.</returns>
        public async IAsyncEnumerable<string> StreamAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            // Replace this implementation with your actual SDK streaming integration.
            // The SDK should yield chunks as soon as they are available.
            for (var i = 1; i <= 10; i++)
            {
                // Stop producing data if the client disconnects.
                cancellationToken.ThrowIfCancellationRequested();

                // Simulate asynchronous work from the SDK.
                await Task.Delay(500, cancellationToken);

                // Each yielded string becomes one SSE event in the controller.
                yield return $"Chunk {i} at {DateTime.UtcNow:O}";
            }
        }
    }
}
