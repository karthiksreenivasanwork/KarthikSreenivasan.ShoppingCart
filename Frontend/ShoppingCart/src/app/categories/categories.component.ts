import { Component, OnInit } from '@angular/core';
import { ProductsService } from '../services/products.service';

/**
 * Displays the list of categories available for each product to help filter data 
 */
@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.css'],
})
export class CategoriesComponent implements OnInit {
  productCategories: any[] = [];

  constructor(public productService: ProductsService) {}

  ngOnInit(): void {
    this.productService.getAllCategories().subscribe({
      next: (data: any) => {
        this.productCategories = data;
      },
      error: (error: any) => {
        console.log(error);
      },
    });
  }
}
