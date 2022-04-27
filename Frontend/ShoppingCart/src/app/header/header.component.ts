import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UsersService } from '../services/users.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit {
  constructor(public usersService: UsersService, public routerRef: Router) {}

  ngOnInit(): void {}

  onLogoutClick(){
    this.usersService.logout();
    this.routerRef.navigateByUrl("/login");
  }
}
