import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { GlobalappdataService } from './globalappdata.service';

/**
 * Service that is responsible for managing data related to the items in the cart.
 */
@Injectable({
  providedIn: 'root',
})
export class CartService {

  constructor(
    public httpClient: HttpClient,
    public globalAppData: GlobalappdataService
  ) {}

  /**
   * API call to get all items from the cart.
   * @returns
   */
  getMyCartItems(): Observable<any> {
    return this.httpClient.get<any[]>(
      `${this.globalAppData.GetApiUrl}/api/v1/Cart/Items`
    );
  }

  /**
   * API call to get the count of all the unique products in the cart.
   * @returns
   */
  getCartItemsCount(): Observable<any> {
    return this.httpClient.get<any[]>(
      `${this.globalAppData.GetApiUrl}/api/v1/Cart/count`
    );
  }

  /**
   * API call to add an product to the cart.
   * @param productID
   * @returns
   */
  addItemsToCart(productID: string): Observable<any> {
    let cartItemDataToAdd = new FormData();
    cartItemDataToAdd.append('ProductID', productID); //ProductID is the name of the property that maps the model defined in the API
    return this.httpClient.post(
      `${this.globalAppData.GetApiUrl}/api/v1/Cart/add`,
      cartItemDataToAdd
    );
  }

  /**
   * API call to remove a single product quanity.
   * @param orderID
   * @param productID
   * @returns
   */
  removeProductQtyFromCart(
    orderID: string,
    productID: string
  ): Observable<any> {
    let cartItemDataToDelete = new FormData();
    cartItemDataToDelete.append('orderID', orderID);
    cartItemDataToDelete.append('productID', productID);
    return this.httpClient.post(
      `${this.globalAppData.GetApiUrl}/api/v1/Cart/removeprodqty`,
      cartItemDataToDelete
    );
  }

  /**
   * API call to removes the entire product with it's quanity.
   * @param orderID
   * @param productID
   * @returns
   */
  removeProductFromCart(orderID: string, productID: string): Observable<any> {
    let cartItemDataToDelete = new FormData();
    cartItemDataToDelete.append('orderID', orderID);
    cartItemDataToDelete.append('productID', productID);
    return this.httpClient.post(
      `${this.globalAppData.GetApiUrl}/api/v1/Cart/removeproduct`,
      cartItemDataToDelete
    );
  }
}
