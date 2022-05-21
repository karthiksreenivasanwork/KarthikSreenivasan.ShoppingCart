import { Component, OnDestroy, OnInit } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { Subscription } from 'rxjs';
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
export class HeaderComponent implements OnInit, OnDestroy {
  readonly searchStringTextboxLabel: string = 'Search a product...';

  cartCount: number = 0;
  searchedProductName: string = '';
  subscriptionCollection: Subscription[] = [];

  constructor(
    private usersService: UsersService,
    private cartService: CartService,
    private compCommunicate: ComponentcommunicationService,
    private routerRef: Router
  ) {
    this.searchedProductName = this.searchStringTextboxLabel;
  }

  ngOnInit(): void {
    this.updateCartCount();
    this.updateCartCountFromAPI();
  }

  ngOnDestroy(): void {
    if (this.subscriptionCollection.length > 0)
      this.subscriptionCollection.forEach((subscription) => {
        subscription.unsubscribe();
      });
  }

  /**
   * Returns true if the user has logged in and false otherwise.
   */
  get getIsUserLoggedIn(): boolean {
    /**
     * Dev Comment: It is a good practice to have the services privately and expose what is necessary to the
     * template via this implementation instead of the using the service reference directly.
     */
    return this.usersService.isUserLoggedIn;
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
        next: (data: number) => {
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
    this.subscriptionCollection.push(
      this.compCommunicate.onupdateCartCountEvent.subscribe({
        next: () => {
          this.updateCartCountFromAPI();
        },
        error: () => {
          console.log('Error while updating the cart count.');
        },
      })
    );
  }

  /**
   * Logout button click event.
   */
  onLogoutClick() {
    this.usersService.logout();
    this.routerRef.navigateByUrl('/login');
    this.cartCount = 0;
  }

  /**
   * Search button click event that navigates to the list products component
   * with the product search text.
   */
  onSearchButtonClick() {
    if (this.searchedProductName != this.searchStringTextboxLabel) {
      let searchedString = this.searchedProductName.trim();
      if (searchedString.length > 0) {
        let navigationExtras: NavigationExtras = {
          state: {
            productSearchText: searchedString,
          },
        };
        this.routerRef.navigate(['listproducts'], navigationExtras);
      } else this.routerRef.navigateByUrl(`/`);
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
