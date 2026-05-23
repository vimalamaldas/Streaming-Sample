using System.Collections.Generic;
using System.Threading;

namespace StreamingSample.API.Services
{
    public interface IStreamingSdk
    {
        IAsyncEnumerable<string> StreamAsync(CancellationToken cancellationToken);
    }
}
