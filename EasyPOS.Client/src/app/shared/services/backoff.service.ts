import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable()
export class BackoffService {
  private lastRefreshTime = 0;
  private initialDelay = 0; // No delay for the first click
  private delayIncrement = 1000; // Increment delay by 1 second
  private maxDelay = 10000; // Maximum delay in milliseconds
  private currentDelay = this.initialDelay;
  private resetPeriod = 5000; // 5 seconds period to reset delay
  private delaySubject = new BehaviorSubject<number>(this.initialDelay);

  delay$ = this.delaySubject.asObservable();
  private jitter = 500; // Max jitter in milliseconds

  getDelay() {
    const currentTime = Date.now();
    const elapsedTime = currentTime - this.lastRefreshTime;

    if (elapsedTime > this.resetPeriod) {
      this.currentDelay = this.initialDelay; // Reset to initial delay after inactivity
    } else {
      this.currentDelay = Math.min(this.currentDelay + this.delayIncrement, this.maxDelay);
      this.currentDelay = this.addJitter(this.currentDelay); // Add unpredictable delay
    }

    this.lastRefreshTime = currentTime;
    this.delaySubject.next(this.currentDelay);

    return this.currentDelay;
  }

  addJitter(delay: number): number {
    let jitter = 0;
    if (delay > 0) {
      jitter = Math.floor(Math.random() * this.jitter); // Random value between 0 and max jitter
    }
    return delay + jitter;
  }

  resetDelay() {
    this.currentDelay = this.initialDelay;
    this.delaySubject.next(this.currentDelay);
  }
}
