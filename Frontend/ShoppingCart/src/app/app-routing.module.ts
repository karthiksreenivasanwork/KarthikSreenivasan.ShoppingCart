import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { ListproductsComponent } from './products/listproducts/listproducts.component';

const routes: Routes = [
  { path: '', component: ListproductsComponent },
  { path: 'login', component: LoginComponent },
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
