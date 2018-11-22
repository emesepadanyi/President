import { Component, OnInit, Input } from '@angular/core';
import { User } from '../models/user.interface';
import { FriendRequestsComponent } from '../friend-requests/friend-requests.component';
import { FindFriendsComponent } from '../find-friends/find-friends.component';

@Component({
  selector: 'app-user-card',
  templateUrl: './user-card.component.html',
  styleUrls: ['./user-card.component.scss']
})
export class UserCardComponent implements OnInit {

  @Input() user: User;

  @Input() request: boolean;
  @Input() requestParent: FriendRequestsComponent;

  @Input() find: boolean;
  @Input() findParent: FindFriendsComponent;
  constructor() { }

  ngOnInit() {
  }

  private accept(userId: string){
    this.requestParent.acceptRequest(userId);
  }

  private reject(userId: string){
    this.requestParent.rejectRequest(userId);
  }

  private send(userId: string){
    this.findParent.sendRequest(userId);
  }
}