import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

const LOCAL_STORAGE_KEY_LOGGED_USER: string = 'loggeduser';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  private _keysInLocalStorage: string[] = [];

  constructor(public httpClient: HttpClient) {
    this._keysInLocalStorage.push(LOCAL_STORAGE_KEY_LOGGED_USER);
  }

  userRegistration(registrationDataFromUser: any): Observable<string> {
    return this.httpClient.post<string>(
      'https://credo-shoppingcartv5.herokuapp.com/register',
      registrationDataFromUser
    );
  }

  userLogin(loginCredentialFromUser: any): Observable<string> {
    return this.httpClient.post<string>(
      'https://credo-shoppingcartv5.herokuapp.com/login',
      loginCredentialFromUser
    );
  }

  /**
   * Returns true if the value is available and false otherwise.
   */
  get isUserLoggedIn(): boolean {
    return !!localStorage.getItem(LOCAL_STORAGE_KEY_LOGGED_USER);
  }

  userLogout() {
    this._keysInLocalStorage.forEach((localStorageKey) => {
      localStorage.removeItem(localStorageKey);
    });
  }
}
