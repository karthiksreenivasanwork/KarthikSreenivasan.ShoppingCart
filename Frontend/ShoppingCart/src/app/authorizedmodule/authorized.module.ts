import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AuthorizedRoutingModule } from './authorized-routing.module';
import { FormsModule } from '@angular/forms';
import { AddproductsComponent } from './products/addproducts/addproducts.component';
import { ViewcartComponent } from './products/viewcart/viewcart.component';

/**
 * Added a new feature module to separate components that need to be loaded only after the user has been successfully authenticated.
 * This was implemented to improve performance using asynchronous loading.
 */
@NgModule({
  declarations: [
    ViewcartComponent,
    AddproductsComponent
  ],
  imports: [
    CommonModule,
    AuthorizedRoutingModule,
    FormsModule
  ]
})
export class AuthorizedModule { }
