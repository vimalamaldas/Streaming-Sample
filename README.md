# Streaming Sample

A full-stack demonstration project showcasing **Server-Sent Events (SSE)** for real-time server to client communication between an ASP.NET Core backend and an Angular frontend.

## 📋 Overview

This project illustrates how to implement efficient real-time data streaming using Server-Sent Events, with a focus on:
- Streaming data chunks immediately as they become available
- Proper HTTP connection lifecycle management
- Client-side consumption of SSE events
- Graceful cancellation handling

## 📁 Project Structure

```
Streaming-Sample/
├── backend/          # ASP.NET Core API with SSE endpoint
├── frontend/         # Angular application consuming SSE events
└── README.md         # This file
```

## 🚀 Quick Start

### Prerequisites
- **.NET SDK** (for backend)
- **Node.js** (for frontend)
- A modern web browser

### Running the Backend

```bash
cd backend
dotnet restore
dotnet run
```

The API will start on `http://localhost:5000` (or the configured port).

### Running the Frontend

```bash
cd frontend
npm install
npm start
```

The application will open in your browser at `http://localhost:4200`.

## 🔧 Backend (C# / ASP.NET Core)

**Location:** `backend/`

### How It Works

- **Endpoint:** `GET /api/streaming/progress`
- **Content-Type:** `text/event-stream; charset=utf-8`
- **Pattern:** Long-lived HTTP connection with streaming responses

### Key Features

- `StreamingController` exposes the SSE endpoint
- `StreamingSdkClient` simulates data production as `IAsyncEnumerable<string>`
- Server flushes each chunk immediately for real-time delivery
- Supports automatic cancellation when client disconnects

### SSE Event Format

```
data: "Chunk 1 at 2026-05-19T..."

data: "Chunk 2 at 2026-05-19T..."

```

Each message is prefixed with `data:` and terminated by a blank line.

### Why IAsyncEnumerable?

- Represents an async stream of values produced over time
- Yields each SDK chunk immediately instead of buffering
- Naturally supports cancellation via `CancellationToken`
- Controller consumes with `await foreach` and sends each event

For detailed information, see [backend/README.md](backend/README.md).

## 💻 Frontend (TypeScript / Angular)

**Location:** `frontend/`

### How It Works

- `streaming.service.ts` creates an `EventSource` connection to the backend
- `streaming.component.ts` subscribes to the service and displays messages in real-time

### Features

- Automatic reconnection on connection loss
- Real-time message rendering
- Observable-based architecture for easy integration with Angular

### Configuration

Update the API URL in `src/app/streaming.component.ts` if your backend is hosted elsewhere:

```typescript
private apiUrl = 'http://localhost:5000/api/streaming/progress';
```

For detailed information, see [frontend/README.md](frontend/README.md).

## 🔄 Architecture

```
┌─────────────────────────┐         HTTP GET          ┌──────────────────────┐
│   Angular Frontend      │──────────────────────────▶│  ASP.NET Core API    │
│                         │                           │                      │
│ - EventSource listener  │◀──────── SSE Stream ──────│ - Streaming endpoint │
│ - Real-time display     │  (text/event-stream)      │ - IAsyncEnumerable   │
│                         │                           │ - Immediate flush    │
└─────────────────────────┘                           └──────────────────────┘
```

## 📚 Learning Resources

- [MDN: Server-Sent Events](https://developer.mozilla.org/en-US/docs/Web/API/Server-sent_events)
- [ASP.NET Core Streaming](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1)
- [Angular EventSource](https://angular.io/api/common/http/HttpClient)

## 🧪 Testing the Streaming Endpoint

You can test the backend endpoint using `curl`:

```bash
curl -N http://localhost:5000/api/streaming/progress
```

The `-N` flag disables buffering to see real-time events.

## ⚙️ Configuration

### Backend
- Port: Configured in `launchSettings.json` (default: `5000`)
- Chunk interval: Configurable in `StreamingSdkClient`

### Frontend
- API URL: Update in `streaming.component.ts`
- Port: Configured in `angular.json` (default: `4200`)

## 📝 Notes

- The backend uses controller-based APIs, not minimal APIs
- Ensure CORS is properly configured if running on different domains
- The frontend must connect using `EventSource` and listen for `onmessage` events
- Both projects include development configurations for easy local testing

## 🔐 CORS Configuration

If frontend and backend are on different origins, configure CORS in the backend's `Startup.cs` or `Program.cs`:

```csharp
services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
```

## 📄 License

This project is provided as-is for educational and demonstration purposes.

## 🤝 Contributing

Feel free to fork, modify, and adapt this sample for your own projects!

---

**Happy streaming! 🌊**
