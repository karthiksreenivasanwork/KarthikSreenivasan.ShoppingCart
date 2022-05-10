import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { GlobalappdataService } from './globalappdata.service';
import { UsersService } from './users.service';

/**
 * Service that is responsible for product management
 */
@Injectable({
  providedIn: 'root',
})
export class ProductsService {
  constructor(
    public httpClient: HttpClient,
    public userService: UsersService,
    public globalAppData: GlobalappdataService
  ) {}

  getAllCategories(): Observable<any> {
    return this.httpClient.get<any[]>(
      `${this.globalAppData.GetApiUrl}/api/v1/Products/categories`
    );
  }

  addProducts(formData: any) {
    return this.httpClient.post(
      `${this.globalAppData.GetApiUrl}/api/v1/Products/add`,
      formData
    );
  }

  getAllProducts() {
    return this.httpClient.get<any[]>(
      `${this.globalAppData.GetApiUrl}/api/v1/Products/products`
    );
  }

  getProductsByCategoryID(categoryIDParam: string) {
    let categoryID: number = Number(categoryIDParam);
    return this.httpClient.get<any[]>(
      `${this.globalAppData.GetApiUrl}/api/v1/Products/products/${categoryID}`
    );
  }
}
