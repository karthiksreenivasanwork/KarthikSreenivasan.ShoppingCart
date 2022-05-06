import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from 'rxjs';

/**
 * Service that manages data related to the items in the cart.
 */
@Injectable({
  providedIn: 'root',
})
export class CartService {
  /*
   * Rxjs: Reactive Extensions for Javascript
   * Rxjs-Subject is a light weight version of the EventEmitter@angularcore concept to send notifications between components.
   */
  //Has to be fired when a product is added to the cart.
  updateCartCountSubj = new Subject();
  //Has to be fired when cart count needs to be reset.
  resetCartSubj = new Subject();

  constructor(public httpClient: HttpClient) {}

  getMyCartItems(): Observable<any> {
    return this.httpClient.get<any[]>(
      'https://localhost:44398/api/v1/Cart/Items'
    );
  }

  getCartItemsCount(): Observable<any> {
    return this.httpClient.get<any[]>(
      'https://localhost:44398/api/v1/Cart/count'
    );
  }

  addItemsToCart(productID: string) {
    let productDataForm = new FormData();
    productDataForm.append('ProductID', productID); //ProductID is the name of the property that maps the model defined in the API
    return this.httpClient.post(
      'https://localhost:44398/api/v1/Cart/add',
      productDataForm
    );
  }

  clearCartItems() {
    this.resetCartSubj.next('');
  }
}
