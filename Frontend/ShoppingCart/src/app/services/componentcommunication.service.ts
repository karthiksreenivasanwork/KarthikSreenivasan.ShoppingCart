import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

/**
 * Service that helps communicate between components
 */
@Injectable({
  providedIn: 'root',
})
export class ComponentcommunicationService {
  /*
   * Rxjs: Reactive Extensions for Javascript
   * Rxjs-Subject is a light weight version of the EventEmitter@angularcore concept to send notifications between components.
   */
  private updateCartCountSubject = new Subject();
  /**
   * Components that want to be notified the cart count is updated..
   */
  onupdateCartCountEvent = this.updateCartCountSubject.asObservable();

  private onSearchKeyUp = new Subject();
  onSearchKeyUpEvent = this.onSearchKeyUp.asObservable();

  /**
   * Method to trigger an event to update the cart count.
   * @param eventArgs
   */
  triggerUpdateCartEvent(eventArgs: string) {
    this.updateCartCountSubject.next(eventArgs);
  }

  triggerSearchKeyUpEvent(productSearchText: string) {
    this.onSearchKeyUp.next(productSearchText);
  }
}
