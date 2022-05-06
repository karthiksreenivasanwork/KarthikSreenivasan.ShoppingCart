import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { UsersService } from './users.service';

@Injectable({
  providedIn: 'root',
})
export class ProductsService {
  constructor(
    public httpClient: HttpClient,
    public userService: UsersService
  ) {}

  getAllCategories(): Observable<any> {
    return this.httpClient.get<any[]>(
      'https://localhost:44398/api/v1/Products/categories'
    );
  }

  addProducts(formData: any) {
    return this.httpClient.post(
      'https://localhost:44398/api/v1/Products/add',
      formData
    );
  }

  getAllProducts() {
    return this.httpClient.get<any[]>(
      'https://localhost:44398/api/v1/Products/products'
    );
  }

  getProductsByCategoryID(categoryIDParam: string) {
    let categoryID: number = Number(categoryIDParam);
    return this.httpClient.get<any[]>(
      `https://localhost:44398/api/v1/Products/products/${categoryID}`
    );
  }
}
