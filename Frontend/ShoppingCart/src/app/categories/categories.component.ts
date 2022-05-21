import { Component, OnInit } from '@angular/core';
import { IProductCategoryModel } from '../models/IProductCategoryModel';
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
  productCategories: IProductCategoryModel[] = [];

  constructor(private productService: ProductsService) {}

  ngOnInit(): void {
    this.productService.getAllCategories().subscribe({
      next: (data: IProductCategoryModel[]) => {
        this.productCategories = data;
      },
      error: (error: any) => {
        console.log(error);
      },
    });
  }
}
