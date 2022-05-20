import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { UsersService } from 'src/app/services/users.service';

/**
 * Performs client-side validation of a logged-in user. 
 * If not applied during routing, the authorization will be handled by HTTP_Interceptors (TokeninterceptorService). 
 */
@Injectable({
  providedIn: 'root',
})
export class ClientauthGuard implements CanActivate {
  constructor(private userService: UsersService, private router: Router) {}

  canActivate(): boolean {
    let isAuthorized: boolean = this.userService.isUserLoggedIn;
    if (!isAuthorized) {
      this.router.navigateByUrl('/login');
    }
    return isAuthorized;
  }
}
