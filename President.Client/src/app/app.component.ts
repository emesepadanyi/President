import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { UserService } from './login/services/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{

  private subscription: Subscription;
  private notification: boolean = false;

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    if(this.userService.isLoggedIn()){
      this.userService._hubConnection.on('Invite', () => {
        this.notification = true;
      });
    }
  }
}
