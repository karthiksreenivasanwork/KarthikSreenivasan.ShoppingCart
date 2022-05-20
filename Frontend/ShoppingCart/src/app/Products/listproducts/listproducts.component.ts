import { Component, OnDestroy, OnInit } from '@angular/core';
import {
  ActivatedRoute,
  NavigationExtras,
  Params,
  Router,
} from '@angular/router';
import { Subscription } from 'rxjs';
import { CartService } from 'src/app/services/cart.service';
import { ComponentcommunicationService } from 'src/app/services/componentcommunication.service';
import { ProductsService } from 'src/app/services/products.service';
import { Filterproducts } from 'src/app/utils/filterproducts';
import { IProductModel } from '../../models/IProductModel';

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
  productModelFilteredCollection: IProductModel[] = [];

  userMessage: string = '';
  userErrorStatus: boolean = false;

  subscriptions: Subscription[] = [];
  productSearchTextInfo: string;

  constructor(
    private productService: ProductsService,
    private cartService: CartService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private compCommunicate: ComponentcommunicationService
  ) {
    const navigation = this.router.getCurrentNavigation();
    const productSearchDetails = navigation.extras as {
      state: {
        productSearchText: string;
      };
    };
    if (navigation.extras.state) {
      this.productSearchTextInfo = productSearchDetails.state.productSearchText;
    }
  }

  ngOnInit(): void {
    this.populateAllProductData();
    this.queryParameterSubscription();
    this.searchTextboxKeyUpEventSubscription();
  }

  /**
   * Populate all the products available.
   */
  populateAllProductData() {
    this.productService.getAllProducts().subscribe({
      next: (data: any) => {
        this.productModelCollection = data as IProductModel[];
        this.productModelFilteredCollection = this.productModelCollection;
      },
      error: (error) => {
        console.log(error);
      },
    });
  }

  /**
   * Subscribe to the query parameters to filter the list of products based on query parameters
   */
  queryParameterSubscription() {
    this.subscriptions.push(
      this.activatedRoute.params.subscribe({
        next: (parameter: Params) => {
          if (parameter['categoryid']) {
            /**
             * Unsubscription is not required for API calls as the subscription gets completed
             * automatically after the HTTPResponse is received from the API.
             */
            this.productService
              .getProductsByCategoryID(parameter['categoryid'])
              .subscribe({
                next: (filteredProducts: any) => {
                  this.productModelFilteredCollection = filteredProducts;
                },
                error: () => {
                  console.log(
                    "Error retrieving product by it's product category"
                  );
                },
              });
          }
        },
        error: (error) => {
          console.log(error);
        },
      })
    );

    //Filter products to display if there are is product search text found in the NavigationExtras parameter for this route.
    if (this.productSearchTextInfo) {
      this.productService
        .getProductsbyName(this.productSearchTextInfo)
        .subscribe({
          next: (data: any) => {
            this.productModelCollection = data as IProductModel[];
            this.productModelFilteredCollection = this.productModelCollection;
            console.log(this.productSearchTextInfo);
            if (this.productModelFilteredCollection.length == 0)
              this.displayNoProductAvailableMessage(this.productSearchTextInfo);
          },
          error: (error) => {
            console.log(error);
          },
        });
    }
  }

  /**
   * Subscribe to the keyup event of the search textbox to filter the products based on their name.
   */
  searchTextboxKeyUpEventSubscription() {
    this.subscriptions.push(
      this.compCommunicate.onSearchKeyUpEvent.subscribe({
        next: (productSearchName: any) => {
          this.applyProductFilter(productSearchName);
          if (this.productModelFilteredCollection.length == 0) {
            this.displayNoProductAvailableMessage(productSearchName);
            this.productModelFilteredCollection = this.productModelCollection;
          }
        },
        error: (error) => {
          console.log(
            'Something went wrong! Unable to filter based on product name'
          );
          console.log(error);
        },
      })
    );
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
        this.compCommunicate.triggerUpdateCartEvent('listproducts');
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
    this.subscriptions.forEach((subscription) => {
      subscription.unsubscribe();
    });
  }

  /**
   * Event handler when a single product is selected
   * @param productData
   */
  onProductSelected(productData: IProductModel) {
    let navigationExtras: NavigationExtras = {
      queryParams: {
        productinfo: JSON.stringify(productData),
      },
    };

    this.router.navigate(['viewproduct'], navigationExtras);
  }

  /**
   * Search event to filter the products based on their name.
   */
  applyProductFilter(productSearchName: string) {
    this.productModelFilteredCollection = Filterproducts.applyFilter(
      this.productModelCollection,
      productSearchName
    );
  }

  /**
   * Display information to the user when no products match the search request.
   */
  displayNoProductAvailableMessage(productSearchName: string) {
    this.userErrorStatus = false;
    this.userMessage = '';

    if (productSearchName.trim() != '') {
      this.userErrorStatus = true;
      this.userMessage = `No products found for the product name : ${productSearchName}`;
      this.productModelFilteredCollection = this.productModelCollection; //Load all the products when the filtered products have no items.
    }
  }
}
