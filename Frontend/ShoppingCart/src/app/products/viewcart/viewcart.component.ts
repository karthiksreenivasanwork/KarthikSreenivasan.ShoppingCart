import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ProductsService } from 'src/app/services/products.service';
import { UsersService } from 'src/app/services/users.service';

@Component({
  selector: 'app-viewcart',
  templateUrl: './viewcart.component.html',
  styleUrls: ['./viewcart.component.css'],
})
export class ViewcartComponent implements OnInit {
  constructor(public userService: UsersService, public productService: ProductsService, public router:Router) {}

  ngOnInit(): void {
    this.productService.getMyCartItems().subscribe(
      {
        next: (data:any[]) =>{
          console.log(data);
        },
        error: (error:any) => {
            console.log(error);
            this.userService.logout();
            this.router.navigateByUrl('/login');
        }
    });
  }
}
