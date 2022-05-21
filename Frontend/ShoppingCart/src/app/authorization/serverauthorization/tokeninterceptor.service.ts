import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, throwError, catchError } from 'rxjs';
import { UsersService } from 'src/app/services/users.service';

/**
 * Attaches a HTTP header with the JWT token for each request to the API
 * after the user has been authenticated.
 *
 * Added server side authorization using HTTP_INTERCEPTORS.
 * Injectable: This is required in the recent versions of Angular even though we are adding them in the providers of the app.module.
 */
@Injectable()
export class TokeninterceptorService implements HttpInterceptor {
  constructor(public userService: UsersService, public router: Router) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    let jwtToken = this.userService.getUserToken;

    //Clone the request and add a HTTPHeader to include the JWT token if available and an empty string otherwise.
    var tokenizedRequest = req.clone({
      setHeaders: {
        Authorization: jwtToken ? `Bearer ${jwtToken}` : '',
      },
    });
    /**
     * Pipe method from Observable:
     * Any data from the server will be sent via pipe method including errors.
     * Example: We can send async data from server for every 10 seconds for some use cases which
     * can be received and processed by pipe.
     */
    return next.handle(tokenizedRequest).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * Handle the error response returned from the API call and then return that HTTPErrorResponse reference to the caller.
   * @param error HttpErrorResponse received from the API call.
   * @returns HttpErrorResponse
   */
  private handleError(error: HttpErrorResponse) {
    if (error && error.status == 401) {
      /**
       * Unless the error is thrown, the subscribers of the API call will not be able to be aware of the error.
       * Status Code - 401 Unauthorized:
       * When the user is found to be unauthorized, logout the user automatically before throwing the error.
       */
      this.userService.logout();
      this.router.navigateByUrl('/login');
      return throwError(() => error);
    }
    return throwError(() => error);
  }
}
