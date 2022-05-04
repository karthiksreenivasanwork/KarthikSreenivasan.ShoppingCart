import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { UsersService } from 'src/app/services/users.service';

/**
 * Performs client side validation of a logged in user.
 * If not applied during routing, the server side validation using HTTP_Interceptors (TokeninterceptorService) is applied. 
 */
@Injectable({
  providedIn: 'root',
})
export class ClientauthGuard implements CanActivate {
  constructor(public userService: UsersService, public router: Router) {}

  canActivate(): boolean {
    let isAuthorized: boolean = this.userService.isUserLoggedIn;
    if (!isAuthorized) {
      this.router.navigateByUrl('/login');
    }
    return isAuthorized;
  }
}
