import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UsersService } from 'src/app/services/users.service';

/**
 * Added server side authorization using HTTP_INTERCEPTORS.
 * Injectable: This is required in the recent versions of Angular even though we are adding them in the providers of the app.module.
 */
@Injectable()
export class TokeninterceptorService implements HttpInterceptor {
  constructor(public userService: UsersService) {}

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
        Authorization: jwtToken
          ? `Bearer ${jwtToken}`
          : '',
      },
    });
    return next.handle(tokenizedRequest); //Send the tokenized request to the server.
  }
}
