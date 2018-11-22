import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FriendsComponent } from './friends/friends.component';
import { RootComponent } from './root/root.component';
import { AuthGuard } from '../auth.guard';

import { routing }  from './friends.routing';
import { FindFriendsComponent } from './find-friends/find-friends.component';
import { FriendRequestsComponent } from './friend-requests/friend-requests.component';
import { UserCardComponent } from './user-card/user-card.component';
import { FriendService } from './services/friend.service';

@NgModule({
  imports: [
    routing,
    CommonModule,
    FormsModule
  ],
  declarations: [FriendsComponent, RootComponent, FindFriendsComponent, FriendRequestsComponent, UserCardComponent],
  providers:    [AuthGuard, FriendService]
})
export class FriendsModule { }
