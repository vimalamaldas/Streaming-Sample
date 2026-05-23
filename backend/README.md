# Streaming Sample API

This backend sample demonstrates an ASP.NET Core controller that exposes a Server-Sent Events (SSE) endpoint.

## How the streaming API works

- The controller is `StreamingController`.
- The streaming endpoint is `GET /api/streaming/progress`.
- The response uses HTTP header `Content-Type: text/event-stream; charset=utf-8`.
- Each message from the SDK is serialized and written as a single SSE event using the `data:` prefix.
- The server flushes each chunk immediately with `Response.Body.FlushAsync` so the frontend can render updates as they arrive.
- The call is long-lived and remains open until the SDK finishes or the client cancels / disconnects.

## SSE event format

The API sends data in this form:

```
data: "Chunk 1 at 2026-05-19T..."

```

Each message is preceded by `data:` and terminated by a blank line.

## SDK wrapper explanation

- `StreamingSdkClient` simulates the underlying SDK and produces data as an `IAsyncEnumerable<string>`.
- Each yielded value is a chunk or event payload that the controller forwards to the client.
- The controller does not buffer the full result; it writes each chunk immediately and flushes the response.
- Use `[EnumeratorCancellation] CancellationToken` so the SDK stream can stop as soon as the client disconnects.

## Why `IAsyncEnumerable<string>`

- `IAsyncEnumerable<T>` represents an asynchronous stream of values produced over time.
- It allows the server to yield each SDK chunk as soon as it becomes available instead of waiting for the whole response.
- The controller can consume the stream with `await foreach` and send each event immediately.
- It also supports cancellation naturally, so the stream can stop cleanly if the client disconnects.

## Cancellation

- The endpoint accepts a `CancellationToken`.
- ASP.NET Core binds this token to the HTTP connection.
- If the client disconnects, the token is triggered and the stream stops.

## Run

1. Open a terminal in `backend`
2. `dotnet restore`
3. `dotnet run`

## Notes

- This sample uses a controller-based API, not minimal APIs.
- Make sure the frontend connects with `EventSource` and reads `onmessage` events.
