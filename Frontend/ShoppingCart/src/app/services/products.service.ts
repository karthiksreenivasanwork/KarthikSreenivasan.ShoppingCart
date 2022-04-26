import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UsersService } from './users.service';

@Injectable({
  providedIn: 'root',
})
export class ProductsService {
  constructor(public httpClient: HttpClient, public userService: UsersService) {}

  getMyCartItems(): Observable<any> {
    console.log(this.userService.getUserToken);
    return this.httpClient.get<any[]>('https://localhost:44398/api/v1/Cart/Items',{
      headers: new HttpHeaders({
        'Authorization' : `Bearer ${this.userService.getUserToken}`
      })
    });
  }
}
