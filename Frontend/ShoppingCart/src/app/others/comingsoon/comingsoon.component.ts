import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-comingsoon',
  templateUrl: './comingsoon.component.html',
  styleUrls: ['./comingsoon.component.css'],
})
export class ComingsoonComponent implements OnInit, OnDestroy {
  pagename: string = '';
  subscriptions: Subscription[] = [];

  constructor(public activeRoute: ActivatedRoute) {}

  ngOnInit(): void {
    this.subscriptions.push(
      this.activeRoute.params.subscribe({
        next: (parameter: Params) => {
          if (parameter['pagename']) {
            this.pagename = parameter['pagename'];
          }
        },
        error: (error) => {
          console.log('Unable to get the page name');
          console.log(error);
        },
      })
    );
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach((subscription) => {
      subscription.unsubscribe();
    });
  }
}
