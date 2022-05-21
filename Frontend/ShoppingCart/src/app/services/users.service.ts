import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
/**
 * jwt-decode is a small browser library that helps to decode JWTs token which is Base64Url encoded.
 */
import jwt_decode from 'jwt-decode';
import { CartService } from './cart.service';
import { GlobalappdataService } from './globalappdata.service';
import { ILoginResultModel } from '../models/Login/ILoginResultModel';
import { IUserModel } from '../models/User/IUserModel';
import { IUserResultModel } from '../models/User/IUserResultModel';

const LOCAL_STORAGE_KEY_LOGGED_USER: string = 'loggeduser';

/**
 * Service that is responsible for user authentication
 */
@Injectable({
  providedIn: 'root',
})
export class UsersService {
  private _keysInLocalStorage: string[] = [];

  constructor(
    private httpClient: HttpClient,
    private cartService: CartService,
    private globalAppData: GlobalappdataService
  ) {
    this._keysInLocalStorage.push(LOCAL_STORAGE_KEY_LOGGED_USER);
  }

  /**
   * API call to create a new user
   * @param registrationDataFromUser Registration details
   * @returns Observable reference of type string.
   */
  registerNewUser(
    registrationDataFromUser: IUserModel
  ): Observable<IUserResultModel> {
    return this.httpClient
      .post<IUserResultModel>(
        `${this.globalAppData.GetApiUrl}/api/v1/users/register`,
        registrationDataFromUser
      )
      .pipe(
        map((response: IUserResultModel) => {
          return response;
        })
      );
  }

  /**
   * API call to validate the credentials
   * @param loginCredentialFromUser Login details
   * @returns Observable reference of type ILoginResultModel.
   */
  userLogin(loginCredentialFromUser: any): Observable<ILoginResultModel> {
    return this.httpClient
      .post<ILoginResultModel>(
        `${this.globalAppData.GetApiUrl}/api/v1/users/login`,
        loginCredentialFromUser
      )
      .pipe(
        map((response: ILoginResultModel) => {
          return response;
        })
      );
  }

  /**
   * Returns the JWT token of the validated user.
   * @returns JWT Token
   */
  get getUserToken(): string {
    return localStorage.getItem(LOCAL_STORAGE_KEY_LOGGED_USER);
  }

  /**
   * Returns true if the value is available and false otherwise.
   *
   * ToDo:
   * Check to see if the JWT key was tampered beyond the basic verification below
   * Please note that we are already verifying the JWT token integrity using HTTPInterceptors.
   */
  get isUserLoggedIn(): boolean {
    let isLoggedIn: boolean = false;

    let jwtToken = localStorage.getItem(LOCAL_STORAGE_KEY_LOGGED_USER);
    if (jwtToken) {
      let jwtDataObject = this.getDecodedAccessToken(jwtToken);
      try {
        //If the username exists, then the local authentication is completed
        isLoggedIn = !!jwtDataObject['unique_name'];
      } catch (Error) {
        //Catch block indicates an invalid token
      }
    }
    return isLoggedIn;
  }

  /**
   * Logout the user by clearing their authenticated data stored locally.
   */
  logout() {
    this._keysInLocalStorage.forEach((localStorageKey) => {
      localStorage.removeItem(localStorageKey);
    });
  }

  /**
   * Decode the JWT token stored in the local storage.
   * @param jwtTokenToVerify JWT token to decode
   */
  getDecodedAccessToken(jwtTokenToVerify: string): any {
    try {
      return jwt_decode(jwtTokenToVerify);
    } catch (Error) {
      //JWT token has been tampered if the catch block is executed
      return null;
    }
  }

  /**
   * Return the username of the authenticated user.
   * @param jwtToken
   * @returns
   */
  getUserName(jwtToken: string) {
    let username: string = '';
    if (this.isUserLoggedIn)
      username = this.getDecodedAccessToken(jwtToken)['unique_name'];
    return username;
  }
}
