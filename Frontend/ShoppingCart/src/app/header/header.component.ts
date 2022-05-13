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
  readonly searchStringTextboxLabel: string = 'Search a product...';
  searchedProductName: string = '';

  constructor(
    public usersService: UsersService,
    public cartService: CartService,
    public compCommunicate: ComponentcommunicationService,
    public routerRef: Router
  ) {
    this.searchedProductName = this.searchStringTextboxLabel;
  }

  ngOnInit(): void {
    this.updateCartCount();
    this.resetCartCount();
    /**
     * Once the login is successful, update the cart item count from the database.
     */
    this.compCommunicate.onSuccessfulLoginEvent.subscribe(() => {
      this.updateCartCountFromAPI();
    });
    /**
     * This would be required on page refresh to update the correct cart items count from the database.
     */
    this.updateCartCountFromAPI();
  }

  /**
   * Update the cart count displayed to the user.
   * 
   * Since this is `Single page application`, the header component will only be loaded for the
   * first time during the browser refresh.
   * Hence, if the count needs to be updated correctly, we need to call this method after
   * every successful authentication from the user.
   */
  updateCartCountFromAPI() {
    if (this.usersService.isUserLoggedIn) {
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

  /**
   * Update the cart count displayed to the user.
   */
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

  /**
   * Reset the cart count displayed to the user.
   */
  resetCartCount() {
    //Reset cart when user logs out
    this.cartService.resetCartSubj.subscribe({
      next: () => {
        this.cartCount = 0;
      },
    });
  }

  /**
   * Logout button click event.
   */
  onLogoutClick() {
    this.usersService.logout();
    this.routerRef.navigateByUrl('/login');
  }

  /**
   * Search button click event that navigates to the list products component
   * with the product search text.
   */
  onSearchButtonClick() {
    if (this.searchedProductName != this.searchStringTextboxLabel) {
      let searchedString = this.searchedProductName.trim();
      if (searchedString.length > 0)
        this.routerRef.navigateByUrl(`listproducts/${searchedString}`);
      else this.routerRef.navigateByUrl(`/`);
    } else this.routerRef.navigateByUrl(`/`);
  }

  /**
   * Search textbox `Focus` event.
   */
  onSearchTextBoxFocus() {
    if (this.searchedProductName == this.searchStringTextboxLabel)
      this.searchedProductName = '';
  }

  /**
   * Search textbox `Blur` event.
   */
  onSearchTextBoxBlur() {
    if (this.searchedProductName == '')
      this.searchedProductName = this.searchStringTextboxLabel;
  }

  /**
   * Search textbox `KeyUp` event.
   */
  onSearchTextBoxKeyUpEvent() {
    this.compCommunicate.triggerSearchKeyUpEvent(this.searchedProductName);
  }
}
