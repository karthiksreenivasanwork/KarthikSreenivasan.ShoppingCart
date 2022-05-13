import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ClientauthGuard } from '../authorization/clientauthorization/clientauth.guard';
import { ComingsoonComponent } from '../others/comingsoon/comingsoon.component';
import { AddproductsComponent } from './products/addproducts/addproducts.component';
import { ViewcartComponent } from './products/viewcart/viewcart.component';

/**
 * Added a new feature module to separate components that need to be loaded only after the user has been successfully authenticated.
 * This was implemented to improve performance using asynchronous loading.
 */
const routes: Routes = [
  {
    path: '',
    children: [
      {
        path: 'viewcart',
        component: ViewcartComponent,
        canActivate: [ClientauthGuard],
      },
      {
        path: 'addproducts',
        component: AddproductsComponent,
        canActivate: [ClientauthGuard],
      },
      { path: 'comingsoon/:pagename', component: ComingsoonComponent },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AuthorizedRoutingModule {}
