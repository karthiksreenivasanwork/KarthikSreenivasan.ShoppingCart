import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { Subscription } from 'rxjs';
import { CartService } from 'src/app/services/cart.service';
import { ProductsService } from 'src/app/services/products.service';
import { IProductModel } from '../IProductModel';

/**
 * Home page of the shopping cart application where all the products
 * available to be purchased.
 */
@Component({
  selector: 'app-listproducts',
  templateUrl: './listproducts.component.html',
  styleUrls: ['./listproducts.component.css'],
})
export class ListproductsComponent implements OnInit, OnDestroy {
  productModelCollection: IProductModel[] = [];

  userMessage: string = '';
  userErrorStatus: boolean = false;

  parameterSubscription: Subscription = null;

  constructor(
    public productService: ProductsService,
    public cartService: CartService,
    public activatedRoute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.parameterSubscription = this.activatedRoute.params.subscribe({
      next: (parameter: Params) => {
        console.log(parameter);
        if (parameter['categoryid']) {
          /**
           * Unsubscription is not required for API calls as the subscription gets completed
           * automatically after the HTTPResponse is received from the API.
           */
          this.productService
            .getProductsByCategoryID(parameter['categoryid'])
            .subscribe({
              next: (filteredProducts: any) => {
                this.productModelCollection = filteredProducts;
              },
              error: () => {
                console.log(
                  "Error retrieving product by it's product category"
                );
              },
            });
        } else {
          /**
           * If no query parameter is found to filter the product based on it's id,
           * then, we display products from all categories.
           */
          this.productService.getAllProducts().subscribe({
            next: (data: any) => {
              this.productModelCollection = data as IProductModel[];
            },
            error: (error) => {
              console.log(error);
            },
          });
        }
      },
      error: (error) => {
        console.log(error);
      },
    });
  }

  /**
   * Event handler when an 'Add to Cart' button is clicked for a product.
   * @param productID Product ID to be added.
   */
  onAddToCartClick(productID: number) {
    this.userMessage = '';
    this.userErrorStatus = false;

    this.cartService.addItemsToCart(productID.toString()).subscribe({
      next: (data: any) => {
        if (data != null) {
          this.userMessage = `Product '${data.productname}' added to cart`;
        } else this.userMessage = 'Product added to cart';
        //Only update the cart count when the cart item has been successfully saved to the database.
        this.cartService.updateCartCountSubj.next('');
      },
      error: (error) => {
        this.userErrorStatus = true;
        this.userMessage = 'Something went wrong!';
        console.log(error);
      },
    });
  }

  /**
   * Dispose of any resources when the component is destroyed
   */
  ngOnDestroy(): void {
    /**
     * Avoids having multiple subscriptions if this component is initialized multiple times.
     */
    this.parameterSubscription.unsubscribe();
  }
}
