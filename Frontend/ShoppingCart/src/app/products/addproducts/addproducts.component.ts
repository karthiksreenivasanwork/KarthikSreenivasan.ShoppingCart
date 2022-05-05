import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { ProductsService } from 'src/app/services/products.service';
import { UsersService } from 'src/app/services/users.service';
import { ProductModel } from '../productmodel';

@Component({
  selector: 'app-addproducts',
  templateUrl: './addproducts.component.html',
  styleUrls: ['./addproducts.component.css'],
})
export class AddproductsComponent implements OnInit {
  userMessage: string;
  userErrorStatus: boolean = false;

  productCategories: any[] = [];
  productImage: any;

  constructor(
    public userService: UsersService,
    public router: Router,
    public productService: ProductsService
  ) {}

  ngOnInit(): void {
    this.productService.getAllCategories().subscribe({
      next: (data: any) => {
        this.productCategories = data;
      },
      error: (error: any) => {
        if (error.status == 401) {
          console.log(error.status);
          this.userService.logout();
          this.router.navigateByUrl('/login');
        }
      },
    });
  }

  addProducts(ngFormTemplateRef: NgForm) {
    this.userErrorStatus = false;
    this.userMessage = '';

    let formData = new FormData();
    formData.append('ProductCategoryName', ngFormTemplateRef.value['category']);
    formData.append('ProductName', ngFormTemplateRef.value['productname']);
    formData.append('ProductPrice', ngFormTemplateRef.value['productprice']);
    formData.append(
      'ProductDescription',
      ngFormTemplateRef.value['productdescription']
    );
    formData.append('ProductImage', this.productImage);
    this.productService.addProducts(formData).subscribe({
      next: (data) => {
        let productModel = data as ProductModel;
        if (productModel != null) {
          if (productModel.productID > 0) {
            this.userMessage = `Product '${productModel.productName}' has been created`;
            ngFormTemplateRef.reset();
          }
        }
      },
      error: (error) => { //Can include errors raised from HTTP Interceptors as well.
        this.userErrorStatus = true;
        //When the status code is 409, the product already exists.
        if (error.status == 409) this.userMessage = error.error;
        else this.userMessage = 'Something went wrong!';
      },
    });
  }

  OnImageUpload(imageData: any) {
    this.productImage = imageData.target.files[0];
  }
}
