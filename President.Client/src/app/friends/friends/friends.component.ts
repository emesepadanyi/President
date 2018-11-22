import { Component, OnInit } from '@angular/core';
import { User } from '../models/user.interface';
import { FriendService } from '../services/friend.service';

@Component({
  selector: 'app-friends',
  templateUrl: './friends.component.html',
  styleUrls: ['./friends.component.css']
})
export class FriendsComponent implements OnInit {
  private myFriends: User[];

  constructor(private friendService: FriendService) { }

  ngOnInit() {
    this.friendService.getFriends().subscribe(friends => { this.myFriends = friends; console.log(friends) });
  }
}
