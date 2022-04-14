import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UsersService } from '../services/users.service';
declare var $: any;

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  userMessage: string;
  userErrorStatus: boolean = false;

  loginForm: FormGroup;

  constructor(public usersService: UsersService, public routerRef: Router) {}

  ngOnInit(): void {
    $('.toggle').click(() => {
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
      Username: new FormControl(null, [Validators.required]),
      Password: new FormControl(null, [Validators.required]),
    });
  }

  doRegistration(ngFormTemplateRef: NgForm) {
    this.usersService.userRegistration(ngFormTemplateRef.value).subscribe({
      next: (registrationResponseData: string) => {
        console.log('User registration response from API');
        console.log(registrationResponseData);

        this.userErrorStatus = false;
        this.userMessage = registrationResponseData;
      },
      error: (registrationErrorData) => {
        console.log('Error during registration process');
        console.log(registrationErrorData);

        this.userMessage = 'Something went wrong!';
        this.userErrorStatus = true;
      },
      complete: () => {
        console.log('User registration completed');
      },
    });
  }

  doLogin() {
    console.log(this.loginForm.value);
    if (this.loginForm.valid) {
      this.usersService.userLogin(this.loginForm.value).subscribe({
        next: (loginResponseData: string) => {
          if (loginResponseData.length == 0) {
            this.userErrorStatus = true;
            this.userMessage = 'Username or password incorrect';
          } else if (loginResponseData.length > 0) {
            localStorage.setItem('loggeduser', loginResponseData);
            this.userErrorStatus = false;
            this.routerRef.navigateByUrl('/'); //When the user has successfully logged in.
          }
        },
        error: (loginErrorData) => {
          console.log('Error during login process');
          console.log(loginErrorData);

          this.userMessage = 'Something went wrong!';
          this.userErrorStatus = true;
        },
      });
    } else {
      this.userErrorStatus = true;
      this.userMessage = 'Username or password incorrect';
    }
  }
}
