import { Component, OnInit } from '@angular/core';
import { ProductsService } from 'src/app/services/products.service';
import { ProductModel } from '../productmodel';

@Component({
  selector: 'app-listproducts',
  templateUrl: './listproducts.component.html',
  styleUrls: ['./listproducts.component.css'],
})
export class ListproductsComponent implements OnInit {
  productModelCollection: ProductModel[] = [];

  constructor(public productService: ProductsService) {}

  ngOnInit(): void {
    this.productService.getAllProducts().subscribe({
      next: (data: any) => {
        this.productModelCollection = data as ProductModel[];
        console.log(data);
      },
      error: (error) => {
        console.log(error);
      },
    });
  }
}
