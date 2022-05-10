import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CartService } from '../services/cart.service';
import { ComponentcommunicationService } from '../services/componentcommunication.service';
import { UsersService } from '../services/users.service';

/**
 * User interface that is represents the top section of this application
 */
@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit {
  cartCount: number = 0;

  constructor(
    public usersService: UsersService,
    public cartService: CartService,
    public compCommunicate: ComponentcommunicationService,
    public routerRef: Router
  ) {}

  ngOnInit(): void {
    this.updateCartCount();
    this.resetCartCount();
    /**
     * Once the login is successful, update the cart item count from the database.
     */
    this.compCommunicate.onSuccessfulLoginEvent.subscribe(()=>{
      this.updateCartCountFromAPI();
    });
    /**
     * This would be required on page refresh to update the correct cart items count from the database.
     */
    this.updateCartCountFromAPI();
  }

  updateCartCountFromAPI() {
    if (this.usersService.isUserLoggedIn) {
      console.log("Calling API from header component to update cart count");
      /**
       * Since this is `Single page application`, the header component will only be loaded for the
       * first time during the browser refresh.
       * Hence, if the count needs to be updated correctly, we need to call this method after
       * every successful authentication from the user.
       */
      this.cartService.getCartItemsCount().subscribe({
        next: (data: any) => {
          this.cartCount = data;
        },
        error: () => {
          console.log('Unable to get the total cart count from the API');
        },
      });
    }
  }

  updateCartCount() {
    //Update cart count if the cart item is saved in the database.
    this.cartService.updateCartCountSubj.subscribe({
      next: () => {
        this.updateCartCountFromAPI();
      },
      error: () => {
        console.log('Error while updating the cart count.');
      },
    });
  }

  resetCartCount() {
    //Reset cart when user logs out
    this.cartService.resetCartSubj.subscribe({
      next: () => {
        this.cartCount = 0;
      },
    });
  }

  onLogoutClick() {
    this.usersService.logout();
    this.routerRef.navigateByUrl('/login');
  }
}
