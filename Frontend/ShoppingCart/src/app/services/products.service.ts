import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { IProductCategoryModel } from '../models/IProductCategoryModel';
import { IProductModel } from '../models/IProductModel';
import { GlobalappdataService } from './globalappdata.service';

/**
 * Service that is responsible for product management
 */
@Injectable({
  providedIn: 'root',
})
export class ProductsService {
  constructor(
    private httpClient: HttpClient,
    private globalAppData: GlobalappdataService
  ) {}

  getAllCategories(): Observable<IProductCategoryModel[]> {
    return this.httpClient
      .get<IProductCategoryModel[]>(
        `${this.globalAppData.GetApiUrl}/api/v1/Products/categories`
      )
      .pipe(
        map((response: IProductCategoryModel[]) => {
          return response;
        })
      );
  }

  addProducts(formData: FormData) {
    return this.httpClient
      .post<IProductModel>(
        `${this.globalAppData.GetApiUrl}/api/v1/Products/add`,
        formData
      )
      .pipe(
        map((response: IProductModel) => {
          return response;
        })
      );
  }

  getAllProducts(): Observable<IProductModel[]> {
    return this.httpClient
      .get<IProductModel[]>(
        `${this.globalAppData.GetApiUrl}/api/v1/Products/products`
      )
      .pipe(
        map((response: IProductModel[]) => {
          return response;
        })
      );
  }

  getProductsbyName(productSearchText: string): Observable<IProductModel[]> {
    return this.httpClient
      .get<IProductModel[]>(
        `${this.globalAppData.GetApiUrl}/api/v1/Products/products/${productSearchText}`
      )
      .pipe(
        map((response: IProductModel[]) => {
          return response;
        })
      );
  }

  getProductsByCategoryID(categoryIDParam: string) {
    let categoryID: number = Number(categoryIDParam);
    return this.httpClient
      .get<IProductModel[]>(
        `${this.globalAppData.GetApiUrl}/api/v1/Products/products/category/${categoryID}`
      )
      .pipe(
        map((response: IProductModel[]) => {
          return response;
        })
      );
  }
}
