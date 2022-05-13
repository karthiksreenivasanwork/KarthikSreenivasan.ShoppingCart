import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { IProductModel } from '../products/IProductModel';

/**
 * Service that helps communicate between components
 */
@Injectable({
  providedIn: 'root',
})
export class ComponentcommunicationService {
  private onLoginSuccessfulSubject = new Subject();
  /**
   * Components that want to be notified on successful login can subscribe to this event.
   */
  onSuccessfulLoginEvent = this.onLoginSuccessfulSubject.asObservable();

  private onSearchKeyUp = new Subject();
  onSearchKeyUpEvent = this.onSearchKeyUp.asObservable();

  /**
   * NOTE: To be triggered by Login component only.
   *
   * Notify components when the login is successful by calling this method after
   * successful authentication of the user.
   * @param message Option parameter that can pass a message as string.
   */
  triggerLoginSuccessfulEvent(message: string) {
    this.onLoginSuccessfulSubject.next(message);
  }

  triggerSearchKeyUpEvent(productSearchText: string) {
    this.onSearchKeyUp.next(productSearchText);
  }
}
