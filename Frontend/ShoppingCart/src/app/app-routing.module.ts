import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';

import { LoginComponent } from './login/login.component';
import { ListproductsComponent } from './products/listproducts/listproducts.component';

import { NotfoundComponent } from './errorhandling/notfound/notfound.component';
import { ViewproductComponent } from './products/viewproduct/viewproduct.component';
import { ComingsoonComponent } from './others/comingsoon/comingsoon.component';

const routes: Routes = [
  { path: '', component: ListproductsComponent },
  { path: 'listproducts', component: ListproductsComponent },
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
  { path: 'viewproduct', component: ViewproductComponent },
  { path: 'login', component: LoginComponent },
  /**
   * Note: The order of asynchronous loading is based on the order of loading each feature module.
   * Hence, order the feature modules based on the use case of the user.
   *
   * Example:
   * Feature module 1: Product module
   * Feature module 2: Cart module
   * Feature module 3: Payment module
   */
  {
    path: 'authorized',
    loadChildren: () =>
      import('./authorizedmodule/authorized.module').then(
        (module) => module.AuthorizedModule
      ),
  },
  { path: 'comingsoon/:pagename', component: ComingsoonComponent },
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
  imports: [
    RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules }), //Asynchronous loading: preloadingStrategy - PreloadAllModules
  ],
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
