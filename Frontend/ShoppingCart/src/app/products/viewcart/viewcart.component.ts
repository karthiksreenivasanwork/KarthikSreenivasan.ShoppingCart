import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CartService } from 'src/app/services/cart.service';
import { ProductsService } from 'src/app/services/products.service';
import { UsersService } from 'src/app/services/users.service';

@Component({
  selector: 'app-viewcart',
  templateUrl: './viewcart.component.html',
  styleUrls: ['./viewcart.component.css'],
})
export class ViewcartComponent implements OnInit {
  constructor(
    public userService: UsersService,
    public productService: ProductsService,
    public cartService: CartService,
    public router: Router
  ) {}

  ngOnInit(): void {
    this.cartService.getMyCartItems().subscribe({
      next: (data: any[]) => {
        console.log(data);
      },
      error: (error: any) => {
        console.log(error);
        /**
         * Added a condition to logout the user only when the status code returns - 401 Unauthorized.
         * It means that the JWT token is invalid.
         * When invalid JWT token is detected, we need to auto-logout the user and send them to the login page.
         */
        if (error.status == 401) { 
          this.userService.logout();
          this.router.navigateByUrl('/login');
        }
      },
    });
  }
}
