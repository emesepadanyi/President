import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { UserService } from './login/services/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy{
  private subscription: Subscription;
  private notification: boolean = false;

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.subscription = this.userService.notification.subscribe(noti => this.notification = noti);
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
