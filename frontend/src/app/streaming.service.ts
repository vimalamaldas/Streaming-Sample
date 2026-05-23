import { Injectable, NgZone } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StreamingService {
  constructor(private ngZone: NgZone) {}

  getStream(url: string): Observable<string> {
    return new Observable<string>(observer => {
      const eventSource = new EventSource(url);
      let streamCompleted = false;

      eventSource.onmessage = event => {
        this.ngZone.run(() => observer.next(event.data));
      };

      eventSource.addEventListener('done', () => {
        streamCompleted = true;
        this.ngZone.run(() => {
          observer.complete();
          eventSource.close();
        });
      });

      eventSource.onerror = () => {
        this.ngZone.run(() => {
          if (streamCompleted) {
            return;
          }

          observer.error('SSE connection error');
          eventSource.close();
        });
      };

      return () => {
        eventSource.close();
      };
    });
  }
}
