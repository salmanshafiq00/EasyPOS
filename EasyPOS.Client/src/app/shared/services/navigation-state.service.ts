import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class NavigationStateService {
  private stateData: any = {};

  setState(key: string, value: any): void {
    this.stateData[key] = value;
  }

  getState(key: string): any {
    const value = this.stateData[key];
    // Optionally clear the state after retrieving
    delete this.stateData[key];
    return value;
  }
}
