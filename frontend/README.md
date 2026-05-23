# Streaming Sample Frontend

This frontend sample contains an Angular service and component that consume a Server-Sent Events (SSE) endpoint.

## Run

1. Open a terminal in `frontend`
2. `npm install`
3. `npm start`

## How it works

- `streaming.service.ts` opens an `EventSource` to the backend SSE endpoint.
- `streaming.component.ts` subscribes to the observable and appends messages as they arrive.

## API URL

Update the URL in `src/app/streaming.component.ts` if your backend is hosted on a different address.
