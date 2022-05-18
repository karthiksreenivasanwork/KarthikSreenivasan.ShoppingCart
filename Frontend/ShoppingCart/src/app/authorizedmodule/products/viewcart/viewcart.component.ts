import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CartService } from 'src/app/services/cart.service';
import { ComponentcommunicationService } from 'src/app/services/componentcommunication.service';
import { ProductsService } from 'src/app/services/products.service';
import { UsersService } from 'src/app/services/users.service';
import { ICartItemCollectionModel } from '../../../products/ICartItemCollectionModel';
import { ICartitemModel } from '../../../products/ICartitemModel';

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
    public userService: UsersService,
    public productService: ProductsService,
    public cartService: CartService,
    public compCommunicate: ComponentcommunicationService,
    public router: Router
  ) {}

  ngOnInit(): void {
    this.displayCartItems();
  }

  displayCartItems() {
    this.resetUserMessageState();
    this.grandTotalAmount = 0;

    this.cartService.getMyCartItems().subscribe({
      next: (data: any[]) => {
        this.cartItemCollection = data as ICartItemCollectionModel[];
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

  removeProductQuantityFromCart(orderID: number, productID: number, index: number) {
    this.resetUserMessageState();

    if (this.cartItemCollection[index].quantity > 0) {
      this.cartService
        .removeProductQtyFromCart(orderID.toString(), productID.toString())
        .subscribe({
          next: (data: any) => {
            let cartModel = data as ICartitemModel;
            if (cartModel != null) {
              this.cartItemCollection[index].quantity -= 1;
              this.cartItemCollection[index].totalAmount =
                cartModel.productPrice *
                this.cartItemCollection[index].quantity;

              if (this.cartItemCollection[index].quantity == 0)
              {
                this.cartItemCollection.splice(index, 1);
                this.compCommunicate.triggerUpdateCartEvent('viewcart');

                this.userDangerAlert = true;
                this.userMessage = `One product quanity of '${cartModel.productname}' removed from cart`;
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

    if (this.cartItemCollection[index].quantity > 0) {
      this.cartService
        .removeProductFromCart(orderID.toString(), productID.toString())
        .subscribe({
          next: (data: any) => {
            console.log(data);

            let cartModel = data as ICartitemModel;
            if (cartModel != null) {
              this.cartItemCollection.splice(index, 1);
              this.compCommunicate.triggerUpdateCartEvent('viewcart');

              this.userDangerAlert = true;
              this.userMessage = `Product '${cartModel.productname}' removed from cart`;

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
      next: (data: any) => {
        let cartModel = data as ICartitemModel;
        if (cartModel != null) {
          this.cartItemCollection[index].quantity += 1;
          this.cartItemCollection[index].totalAmount =
            cartModel.productPrice * this.cartItemCollection[index].quantity;
          this.updateTotalAmount();
        }
        this.userMessage = `Product '${cartModel.productname}' added to cart`;
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
        this.grandTotalAmount += cartItem.totalAmount;
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
}
