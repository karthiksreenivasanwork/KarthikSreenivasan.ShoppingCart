import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ILoginModel } from '../products/ILoginModel';
import { CartService } from '../services/cart.service';
import { ComponentcommunicationService } from '../services/componentcommunication.service';
import { UsersService } from '../services/users.service';
declare var $: any;

/**
 * User interface to authenicate a user.
 */
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  userMessage: string;
  userErrorStatus: boolean = false;

  loginForm: FormGroup;
  @ViewChild('formTemplateRef') registrationForm: NgForm;

  constructor(
    public usersService: UsersService,
    public cartService: CartService,
    public compCommunicate: ComponentcommunicationService,
    public router: Router
  ) {}

  ngOnInit(): void {
    $('.toggle').click(() => {
      this.resetComponentVariables();
      this.resetComponentForms();

      // Switches the Icon
      $(this).children('i').toggleClass('fa-pencil');
      // Switches the forms
      $('.form').animate(
        {
          height: 'toggle',
          'padding-top': 'toggle',
          'padding-bottom': 'toggle',
          opacity: 'toggle',
        },
        'slow'
      );
    });

    this.loginForm = new FormGroup({
      /**
       * Since these are input controls as a part of the login form, the user only needs
       * to enter the values for username and password.
       */
      loginusername: new FormControl('', [Validators.required]),
      loginpassword: new FormControl('', [Validators.required]),
    });
  }

  doRegistration() {
    this.resetComponentVariables();

    this.usersService.registerNewUser(this.registrationForm.value).subscribe({
      next: (registrationResponseData: string) => {
        this.userErrorStatus = false;
        this.userMessage = registrationResponseData;

        console.log(this.userMessage);
        /**
         * This is to indicate the user about the successful registration for a second
         * before redirecting the user to the login page.
         */
        setTimeout(() => {
          //Will reload the page to load the login section as login and registration are on the same component.
          window.location.reload();
        }, 1000);
      },
      error: (registrationErrorData: HttpErrorResponse) => {
        console.log('Error during registration process');
        this.userErrorStatus = true;

        if (registrationErrorData.status == 409) {
          //Username already taken.
          this.userMessage = registrationErrorData.error;
        } else this.userMessage = 'Something went wrong!';
      },
    });
  }

  doLogin() {
    this.resetComponentVariables();

    console.log(this.loginForm.value);

    if (this.loginForm.valid) {
      this.usersService
        .userLogin(this.getLoginModel(this.loginForm))
        .subscribe({
          next: (loginResponseData: string) => {
            if (loginResponseData.length == 0) {
              this.userErrorStatus = true;
              this.userMessage = 'Username or password incorrect';
            } else if (loginResponseData.length > 0) {
              localStorage.setItem('loggeduser', loginResponseData);
              this.userErrorStatus = false;

              this.compCommunicate.triggerUpdateCartEvent('login');
              this.router.navigateByUrl('/');
            }
          },
          error: (loginErrorData: HttpErrorResponse) => {
            this.userErrorStatus = true;
            console.log('Error during login process');
            if (loginErrorData.status == 401) {
              //Unauthorized user.
              this.userMessage = loginErrorData.error;
            } else this.userMessage = 'Something went wrong!';

            this.loginForm.reset();
          },
        });
    } else {
      this.userErrorStatus = true;
      this.userMessage = 'Username or password incorrect';
    }
  }

  /**
   * Extract the form data and convert it to the login model.
   * @param ngLoginForm FormGroup reference
   * @returns ILoginModel
   */
  getLoginModel(ngLoginForm: FormGroup) {
    let loginModel: ILoginModel = {
      username: ngLoginForm.value.loginusername,
      password: ngLoginForm.value.loginpassword,
    };
    return loginModel;
  }

  /**
   * Reset component variables to their original state.
   */
  resetComponentVariables() {
    this.userErrorStatus = false;
    this.userMessage = '';
  }

  /**
   * Reset form variables to their original state.
   */
  resetComponentForms() {
    this.loginForm.reset();
    this.registrationForm.reset();
  }

  //#region Login Textbox Events
  onInputLoginUsernameKeyUp() {
    this.resetComponentVariables();
  }

  onInputLoginPasswordKeyUp() {
    this.resetComponentVariables();
  }
  //#endregion

  //#region Registration Textbox Events
  onInputRegisterUsernameKeyUp() {
    this.resetComponentVariables();
  }

  onInputRegisterPasswordKeyUp() {
    this.resetComponentVariables();
  }

  onInputRegisterEmailKeyUp() {
    this.resetComponentVariables();
  }

  onInputRegisterPhoneKeyUp(){
    this.resetComponentVariables();
  }
  //#endregion
}
