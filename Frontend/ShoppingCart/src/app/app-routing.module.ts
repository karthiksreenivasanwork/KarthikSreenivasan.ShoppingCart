import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LoginComponent } from './login/login.component';
import { ListproductsComponent } from './products/listproducts/listproducts.component';
import { ViewcartComponent } from './products/viewcart/viewcart.component';

import { ClientauthGuard } from './authorization/clientauthorization/clientauth.guard';
import { AddproductsComponent } from './products/addproducts/addproducts.component';
import { NotfoundComponent } from './errorhandling/notfound/notfound.component';

const routes: Routes = [
  { path: '', component: ListproductsComponent },
  /**
   * pathMatch: When the full path is ending with the route - categories,
   * then you redirect to list all the products.
   */
  { path: 'categories', redirectTo: '/', pathMatch: 'full' },
  /**
   * Filter each product using it's category using dynamic routing.
   * Dynamic routing is implemented using query parameters.
   */
  { path: 'categories/:categoryid', component: ListproductsComponent },
  { path: 'login', component: LoginComponent },
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
  /**
   * Wild card route must be defined after all the known routes have been defined.
   * If no route matches, we display the page not found error to the user.
   */
  { path: '**', component: NotfoundComponent },
];

@NgModule({
  /**
   * forRoot(routes) - Sepcifies that the routing is targetting only the root module.
   * forChild(routes) - Specifies that these are for the feature modules.
   */
  imports: [RouterModule.forRoot(routes)],
  /**
   * Library related export. It supports features like
   * 1. Router module directive: <router-outlet></router-outlet>
   *    That means the routes defined in the const variable 'const routes', will be exposed to another module
   *    which is nothing but app.module.ts.
   *    Without exporting (eports: [RouterModule]) RouterModule, angular will not know what this directive is.
   */
  exports: [RouterModule],
})
/**
 * Link/Wiring - Class visibility related export to inform app.module that such a routing exists.
 */
export class AppRoutingModule {}
