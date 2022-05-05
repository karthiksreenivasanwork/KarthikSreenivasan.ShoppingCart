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
    console.log('Interceptor called for this request');
    let jwtToken = this.userService.getUserToken;
    console.log(`JWT Token Data : ${jwtToken}`);

    //Clone the request and add a HTTPHeader to include the JWT token if available and an empty string otherwise.
    var tokenizedRequest = req.clone({
      setHeaders: {
        Authorization: jwtToken ? `Bearer ${jwtToken}` : '',
      },
    });
    return next.handle(tokenizedRequest).pipe(  //Send the tokenized request to the server.
      /**
       * Unless the error is thrown, the subscribers of the API call will not be able to be aware of the error.
       * Status Code - 401 Unauthorized:
       * When the user is found to be unauthorized, logout the user automatically before throwing the error.
       * Throw generic error for other error types as - `Something went wrong!`
       */
      catchError((error: HttpErrorResponse) => {
        if (error && error.status == 401) {
          console.log('Interceptor received invalid token error');
          this.userService.logout();
          this.router.navigateByUrl('/login');
          return throwError(() => new Error('401 Unauthorized'));
        }
        return throwError(() => new Error('Something went wrong!'));
      })
    );
  }
}