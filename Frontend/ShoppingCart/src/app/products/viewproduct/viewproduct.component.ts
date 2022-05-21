import { Component, OnInit } from '@angular/core';
import { IProductModel } from '../../models/Product/IProductModel';
import { Router } from '@angular/router';
import { CartService } from 'src/app/services/cart.service';
import { ComponentcommunicationService } from 'src/app/services/componentcommunication.service';
import { ProductsService } from 'src/app/services/products.service';
import { ICartitemModel } from 'src/app/models/Cart/ICartitemModel';

/**
 * Displays unique product information to the user.
 */
@Component({
  selector: 'app-viewproduct',
  templateUrl: './viewproduct.component.html',
  styleUrls: ['./viewproduct.component.css'],
})
export class ViewproductComponent implements OnInit {
  userMessage: string = '';
  userErrorStatus: boolean = false;

  public productNameFromNav: string;
  public productModel: IProductModel;

  constructor(
    private router: Router,
    private cartService: CartService,
    private productService: ProductsService,
    private compCommunicate: ComponentcommunicationService
  ) {
    this.productModel = {
      ProductCategoryID: 0,
      ProductCategoryName: '',
      ProductDescription: '',
      ProductID: 0,
      ProductImageName: '',
      ProductImageURL: '',
      ProductName: '',
      ProductPrice: 0,
    };

    const navigation = this.router.getCurrentNavigation();
    const navigationData = navigation.extras as {
      state: {
        productName: string;
      };
    };

    if (navigationData)
      this.productNameFromNav = navigationData.state.productName;
  }

  ngOnInit(): void {
    if (this.productNameFromNav) {
      this.productService.getProductsbyName(this.productNameFromNav).subscribe({
        next: (data: IProductModel[]) => {
          if (data.length > 0) {
            this.productModel = data[0];
          }
        },
      });
    }
  }

  /**
   * Event handler when an 'Add to Cart' button is clicked for a product.
   * @param productID Product ID to be added.
   */
  onAddToCartClick(productID: number) {
    this.userMessage = '';
    this.userErrorStatus = false;

    this.cartService.addItemsToCart(productID.toString()).subscribe({
      next: (data: ICartitemModel) => {
        if (data != null) {
          this.userMessage = `Product '${data.Productname}' added to cart`;
        } else this.userMessage = 'Product added to cart';
        //Only update the cart count when the cart item has been successfully saved to the database.
        this.compCommunicate.triggerUpdateCartEvent('viewproducts');
      },
      error: (error) => {
        this.userErrorStatus = true;
        this.userMessage = 'Something went wrong!';
        console.log(error);
      },
    });
  }
}
