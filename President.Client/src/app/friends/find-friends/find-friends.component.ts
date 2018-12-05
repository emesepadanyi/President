import { Component, OnInit } from '@angular/core';
import { User } from '../models/user.interface';
import { FriendService } from '../services/friend.service';

@Component({
  selector: 'app-find-friends',
  templateUrl: './find-friends.component.html',
  styleUrls: ['./find-friends.component.css']
})
export class FindFriendsComponent implements OnInit {

  private key: string;
  private hits: User[];
  constructor(private friendService: FriendService) { }

  ngOnInit() {
  }

  private find(){
    console.log(this.key);
    this.friendService.findUsers(this.key).subscribe(resp => {this.hits = resp; console.log(resp);});
  }

  sendRequest(userId: string){
    this.friendService.sendRequest(userId).subscribe(() => { this.hits = this.hits.filter(obj => obj.id !== userId); });
  }
}
