using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using StreamingSample.API.Services;

namespace StreamingSample.API.Controllers
{
    /// <summary>
    /// Exposes server-sent event (SSE) streaming endpoints.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class StreamingController : ControllerBase
    {
        private readonly IStreamingService _streamingService;

        public StreamingController(IStreamingService streamingService)
        {
            _streamingService = streamingService;
        }

        /// <summary>
        /// Streams progress updates as Server-Sent Events (SSE).
        /// </summary>
        /// <remarks>
        /// This endpoint keeps the HTTP connection open and sends each item from the streaming SDK as
        /// a separate SSE event. The response uses "text/event-stream" and flushes each chunk immediately.
        /// </remarks>
        /// <param name="cancellationToken">Automatically cancels streaming when the client disconnects.</param>
        /// <returns>An open stream that writes events until the SDK completes or the client disconnects.</returns>
        [HttpGet("progress")]
        public async Task GetProgress(CancellationToken cancellationToken)
        {
            // Prevents client-side caching so the browser receives fresh streaming data.
            Response.Headers.Add("Cache-Control", "no-cache");

            // Disables buffering in proxies like nginx so events are delivered immediately.
            Response.Headers.Add("X-Accel-Buffering", "no");

            // Tell the client this is a Server-Sent Events stream.
            Response.ContentType = "text/event-stream; charset=utf-8";

            // Iterate over the async stream from the service as new chunks arrive.
            await foreach (var chunk in _streamingService.GetStreamAsync(cancellationToken))
            {
                // If the client disconnected, stop sending additional events.
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                // Format the chunk as an SSE event payload.
                // Each event line begins with "data:" and a blank line ends the event.
                var payload = $"data: {JsonSerializer.Serialize(chunk)}\n\n";

                // Convert the payload text into UTF-8 bytes for the HTTP response body.
                var bytes = Encoding.UTF8.GetBytes(payload);

                // Write the event bytes to the response stream.
                await Response.Body.WriteAsync(bytes, cancellationToken);

                // Flush the body so the event is sent immediately instead of buffered.
                await Response.Body.FlushAsync(cancellationToken);
            }

            // Notify the client that the stream has completed cleanly.
            var donePayload = "event: done\ndata:\n\n";
            var doneBytes = Encoding.UTF8.GetBytes(donePayload);
            await Response.Body.WriteAsync(doneBytes, cancellationToken);
            await Response.Body.FlushAsync(cancellationToken);
        }
    }
}
