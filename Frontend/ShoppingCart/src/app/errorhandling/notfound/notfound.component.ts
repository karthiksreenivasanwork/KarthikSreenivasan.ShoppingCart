import { Component, OnInit } from '@angular/core';

/**
 * When a user navigates to an unknown route, it is directed to this page.
 */
@Component({
  selector: 'app-notfound',
  templateUrl: './notfound.component.html',
  styleUrls: ['./notfound.component.css']
})
export class NotfoundComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}
