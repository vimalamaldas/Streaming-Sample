using System.Collections.Generic;
using System.Threading;

namespace StreamingSample.API.Services
{
    public class StreamingService : IStreamingService
    {
        // Holds the underlying SDK wrapper that produces streaming data.
        private readonly IStreamingSdk _sdk;

        // Constructor injection of the SDK wrapper.
        public StreamingService(IStreamingSdk sdk)
        {
            _sdk = sdk;
        }

        // Exposes a streaming API to the controller.
        // This method does nothing more than forward the cancellation token
        // and the async enumerable from the SDK wrapper.
        public IAsyncEnumerable<string> GetStreamAsync(CancellationToken cancellationToken)
        {
            return _sdk.StreamAsync(cancellationToken);
        }
    }
}
