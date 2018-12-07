import { Component, OnInit } from '@angular/core';
import { User } from '../models/user.interface';
import { FriendService } from '../services/friend.service';

@Component({
  selector: 'app-friend-requests',
  templateUrl: './friend-requests.component.html',
  styleUrls: ['./friend-requests.component.css']
})
export class FriendRequestsComponent implements OnInit {
  private myRequests: User[];

  constructor(private friendService: FriendService) { }

  ngOnInit() {
    this.updateMyRequests();
  }

  updateMyRequests() {
    this.friendService.getRequests().subscribe(requests => { this.myRequests = requests; console.log(requests) });
  }

  acceptRequest(userId: string) {
    this.friendService.acceptRequest(userId)
      .subscribe(() => {
        //TODO nice notification
        this.updateMyRequests();
      });
  }

  rejectRequest(userId: string) {
    this.friendService.rejectRequest(userId).subscribe(() => {
      //TODO nice notification
      this.updateMyRequests();
    });
  }
}
