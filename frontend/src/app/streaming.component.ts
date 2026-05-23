import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { StreamingService } from './streaming.service';

@Component({
  selector: 'app-streaming',
  templateUrl: './streaming.component.html',
  standalone: true,
  imports: [CommonModule]
})
export class StreamingComponent implements OnInit, OnDestroy {
  messages: string[] = [];
  error?: string;
  streamCompleted = false;
  private destroy$ = new Subject<void>();

  constructor(private streamingService: StreamingService) {}

  ngOnInit(): void {
    const url = 'http://localhost:5000/api/streaming/progress';

    this.streamingService.getStream(url)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: message => {
          this.error = undefined;
          this.messages.push(message);
        },
        error: err => this.error = err,
        complete: () => {
          this.streamCompleted = true;
          this.error = undefined;
        }
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
