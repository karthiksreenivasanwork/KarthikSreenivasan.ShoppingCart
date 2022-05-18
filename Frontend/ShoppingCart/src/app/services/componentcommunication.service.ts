import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

/**
 * Service that helps communicate between components
 */
@Injectable({
  providedIn: 'root',
})
export class ComponentcommunicationService {
  private onSearchKeyUp = new Subject();
  onSearchKeyUpEvent = this.onSearchKeyUp.asObservable();

  triggerSearchKeyUpEvent(productSearchText: string) {
    this.onSearchKeyUp.next(productSearchText);
  }
}
