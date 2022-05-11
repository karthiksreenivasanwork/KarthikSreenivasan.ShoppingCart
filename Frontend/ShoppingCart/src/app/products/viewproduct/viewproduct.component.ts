import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { IProductModel } from '../IProductModel';
import { ActivatedRoute } from '@angular/router';

/**
 * Displays unique product information to the user.
 */
@Component({
  selector: 'app-viewproduct',
  templateUrl: './viewproduct.component.html',
  styleUrls: ['./viewproduct.component.css'],
})
export class ViewproductComponent implements OnInit, OnDestroy {
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

  constructor(public activeRoute: ActivatedRoute) {}

  ngOnInit(): void {
    this.parameterSubscription = this.activeRoute.queryParams.subscribe(params => {
      this.productModel = JSON.parse(params["productinfo"]);
   });
  }

  ngOnDestroy(): void {
    this.parameterSubscription.unsubscribe();
  }
}
