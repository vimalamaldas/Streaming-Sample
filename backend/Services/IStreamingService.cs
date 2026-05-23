using System.Collections.Generic;
using System.Threading;

namespace StreamingSample.API.Services
{
    public interface IStreamingService
    {
        IAsyncEnumerable<string> GetStreamAsync(CancellationToken cancellationToken);
    }
}
