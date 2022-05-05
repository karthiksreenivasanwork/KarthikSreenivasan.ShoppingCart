import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
import { CategoriesComponent } from './categories/categories.component';
import { ListproductsComponent } from './products/listproducts/listproducts.component';
import { FooterComponent } from './footer/footer.component';
import { LoginComponent } from './login/login.component';
import { ViewcartComponent } from './products/viewcart/viewcart.component';
import { TokeninterceptorService } from './authorization/serverauthorization/tokeninterceptor.service';
import { AddproductsComponent } from './products/addproducts/addproducts.component';
import { ShowminimumproddescPipe } from './custompipes/showminimumproddesc.pipe';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    CategoriesComponent,
    ListproductsComponent,
    FooterComponent,
    LoginComponent,
    ViewcartComponent,
    AddproductsComponent,
    ShowminimumproddescPipe,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokeninterceptorService,
      multi: true, //Mandatory property - Specifies if other interceptors must be called if available
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
