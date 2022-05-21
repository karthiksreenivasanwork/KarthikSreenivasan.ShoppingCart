import { Component, OnInit } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { CartService } from 'src/app/services/cart.service';
import { ComponentcommunicationService } from 'src/app/services/componentcommunication.service';
import { ProductsService } from 'src/app/services/products.service';
import { UsersService } from 'src/app/services/users.service';
import { ICartItemCollectionModel } from '../../../models/Cart/ICartItemCollectionModel';
import { ICartitemModel } from '../../../models/Cart/ICartitemModel';

@Component({
  selector: 'app-viewcart',
  templateUrl: './viewcart.component.html',
  styleUrls: ['./viewcart.component.css'],
})
export class ViewcartComponent implements OnInit {
  cartItemCollection: ICartItemCollectionModel[] = [];
  grandTotalAmount: number = 0;

  userMessage: string = '';
  userErrorStatus: boolean = false;
  userDangerAlert: boolean = false;

  constructor(
    private userService: UsersService,
    private productService: ProductsService,
    private cartService: CartService,
    private compCommunicate: ComponentcommunicationService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.displayCartItems();
  }

  displayCartItems() {
    this.resetUserMessageState();
    this.grandTotalAmount = 0;

    this.cartService.getMyCartItems().subscribe({
      next: (data: ICartItemCollectionModel[]) => {
        this.cartItemCollection = data;
        this.informUserWhenCartisEmpty();
        this.updateTotalAmount();
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

  removeProductQuantityFromCart(
    orderID: number,
    productID: number,
    index: number
  ) {
    this.resetUserMessageState();

    if (this.cartItemCollection[index].Quantity > 0) {
      this.cartService
        .removeProductQtyFromCart(orderID.toString(), productID.toString())
        .subscribe({
          next: (data: ICartitemModel) => {
            let cartModel: ICartitemModel = data;
            if (cartModel != null) {
              this.cartItemCollection[index].Quantity -= 1;
              this.cartItemCollection[index].TotalAmount =
                cartModel.ProductPrice *
                this.cartItemCollection[index].Quantity;

              if (this.cartItemCollection[index].Quantity == 0) {
                this.cartItemCollection.splice(index, 1);
                this.compCommunicate.triggerUpdateCartEvent('viewcart');

                this.userDangerAlert = true;
                this.userMessage = `One product quanity of '${cartModel.Productname}' removed from cart`;
              }
              this.updateTotalAmount();
            }
          },
          error: () => {
            this.userErrorStatus = true;
            this.userMessage = 'Something went wrong';
            console.log('Unable to remove item from cart.');
          },
        });
    }
    this.informUserWhenCartisEmpty();
  }

  removeProductFromCart(orderID: number, productID: number, index: number) {
    this.resetUserMessageState();

    if (this.cartItemCollection[index].Quantity > 0) {
      this.cartService
        .removeProductFromCart(orderID.toString(), productID.toString())
        .subscribe({
          next: (data: ICartitemModel) => {
            let cartModel: ICartitemModel = data;
            if (cartModel != null) {
              this.cartItemCollection.splice(index, 1);
              this.compCommunicate.triggerUpdateCartEvent('viewcart');

              this.userDangerAlert = true;
              this.userMessage = `Product '${cartModel.Productname}' removed from cart`;

              this.updateTotalAmount();
            }
          },
          error: () => {
            this.userErrorStatus = true;
            this.userMessage = 'Something went wrong';
            console.log('Unable to remove product from cart.');
          },
        });
    }
    this.informUserWhenCartisEmpty();
  }

  addItemQtyToCart(productID: number, index: number) {
    this.resetUserMessageState();

    this.cartService.addItemsToCart(productID.toString()).subscribe({
      next: (data: ICartitemModel) => {
        let cartModel = data;
        if (cartModel != null) {
          this.cartItemCollection[index].Quantity += 1;
          this.cartItemCollection[index].TotalAmount =
            cartModel.ProductPrice * this.cartItemCollection[index].Quantity;
          this.updateTotalAmount();
        }
        this.userMessage = `Product '${cartModel.Productname}' added to cart`;
      },
      error: () => {
        this.userErrorStatus = true;
        this.userMessage = 'Something went wrong';
        console.log('Unable to add items to cart.');
      },
    });
  }

  updateTotalAmount() {
    this.grandTotalAmount = 0;
    if (this.cartItemCollection != null && this.cartItemCollection.length > 0) {
      this.cartItemCollection.forEach((cartItem) => {
        this.grandTotalAmount += cartItem.TotalAmount;
      });
    }
  }

  resetUserMessageState() {
    this.userMessage = '';
    this.userErrorStatus = false;
    this.userDangerAlert = false;
  }

  informUserWhenCartisEmpty() {
    this.userDangerAlert = false;
    if (this.cartItemCollection.length == 0) {
      this.userDangerAlert = true;
      this.userMessage = 'Cart is empty. Please place an order.';
    }
  }

  /**
   * Event handler when a single product is selected
   * @param productData
   */
  onProductSelected(cartItem: ICartItemCollectionModel) {
    console.log(cartItem.Productname);
    let navigationData: NavigationExtras = {
      state: {
        productName: cartItem.Productname,
      },
    };

    this.router.navigate(['viewproduct'], navigationData);
  }
}
