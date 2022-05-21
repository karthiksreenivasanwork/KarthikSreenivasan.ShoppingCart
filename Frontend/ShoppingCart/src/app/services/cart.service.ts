import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { ICartItemCollectionModel } from '../models/Cart/ICartItemCollectionModel';
import { ICartitemModel } from '../models/Cart/ICartitemModel';
import { GlobalappdataService } from './globalappdata.service';

/**
 * Service that is responsible for managing data related to the items in the cart.
 */
@Injectable({
  providedIn: 'root',
})
export class CartService {
  constructor(
    private httpClient: HttpClient,
    private globalAppData: GlobalappdataService
  ) {}

  /**
   * API call to get all items from the cart.
   * @returns An array of ICartItemCollectionModel
   */
  getMyCartItems(): Observable<ICartItemCollectionModel[]> {
    return this.httpClient
      .get<ICartItemCollectionModel[]>(
        `${this.globalAppData.GetApiUrl}/api/v1/Cart/Items`
      )
      .pipe(
        map((response: ICartItemCollectionModel[]) => {
          return response;
        })
      );
  }

  /**
   * API call to get the count of all the unique products in the cart.
   * @returns Cart count as number
   */
  getCartItemsCount(): Observable<number> {
    return this.httpClient
      .get<number>(`${this.globalAppData.GetApiUrl}/api/v1/Cart/count`)
      .pipe(
        map((response: number) => {
          return response;
        })
      );
  }

  /**
   * API call to add an product to the cart.
   * @param productID
   * @returns ICartitemModel
   */
  addItemsToCart(productID: string): Observable<ICartitemModel> {
    let cartItemDataToAdd = new FormData();
    cartItemDataToAdd.append('ProductID', productID); //ProductID is the name of the property that maps the model defined in the API
    return this.httpClient
      .post<ICartitemModel>(
        `${this.globalAppData.GetApiUrl}/api/v1/Cart/add`,
        cartItemDataToAdd
      )
      .pipe(
        map((response) => {
          return response;
        })
      );
  }

  /**
   * API call to remove a single product quanity.
   * @param orderID
   * @param productID
   * @returns ICartitemModel
   */
  removeProductQtyFromCart(
    orderID: string,
    productID: string
  ): Observable<ICartitemModel> {
    let cartItemDataToDelete = new FormData();
    cartItemDataToDelete.append('orderID', orderID);
    cartItemDataToDelete.append('productID', productID);
    return this.httpClient
      .post<ICartitemModel>(
        `${this.globalAppData.GetApiUrl}/api/v1/Cart/removeprodqty`,
        cartItemDataToDelete
      )
      .pipe(
        map((response: ICartitemModel) => {
          return response;
        })
      );
  }

  /**
   * API call to removes the entire product with it's quanity.
   * @param orderID
   * @param productID
   * @returns
   */
  removeProductFromCart(
    orderID: string,
    productID: string
  ): Observable<ICartitemModel> {
    let cartItemDataToDelete = new FormData();
    cartItemDataToDelete.append('orderID', orderID);
    cartItemDataToDelete.append('productID', productID);
    return this.httpClient
      .post<ICartitemModel>(
        `${this.globalAppData.GetApiUrl}/api/v1/Cart/removeproduct`,
        cartItemDataToDelete
      )
      .pipe(
        map((response: ICartitemModel) => {
          return response;
        })
      );
  }
}
