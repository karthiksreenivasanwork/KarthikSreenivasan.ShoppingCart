import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { IProductModel } from '../IProductModel';
import { ActivatedRoute } from '@angular/router';
import { CartService } from 'src/app/services/cart.service';

/**
 * Displays unique product information to the user.
 */
@Component({
  selector: 'app-viewproduct',
  templateUrl: './viewproduct.component.html',
  styleUrls: ['./viewproduct.component.css'],
})
export class ViewproductComponent implements OnInit, OnDestroy {
  userMessage: string = '';
  userErrorStatus: boolean = false;

  public productModel: IProductModel = {
    productCategoryID: 0,
    productCategoryName: '',
    productDescription: '',
    productID: 0,
    productImageName: '',
    productImageURL: '',
    productName: '',
    productPrice: 0,
  };

  public parameterSubscription: Subscription;

  constructor(
    public activeRoute: ActivatedRoute,
    public cartService: CartService
  ) {}

  ngOnInit(): void {
    this.parameterSubscription = this.activeRoute.queryParams.subscribe(
      (params) => {
        this.productModel = JSON.parse(params['productinfo']);
      }
    );
  }

  ngOnDestroy(): void {
    this.parameterSubscription.unsubscribe();
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
        this.cartService.triggerUpdateCartEvent('viewproducts');
      },
      error: (error) => {
        this.userErrorStatus = true;
        this.userMessage = 'Something went wrong!';
        console.log(error);
      },
    });
  }
}
