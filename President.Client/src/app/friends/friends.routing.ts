import { AuthGuard } from '../auth.guard';
import { ModuleWithProviders } from '@angular/core';
import { RouterModule } from '@angular/router';
import { RootComponent } from './root/root.component';
import { FriendsComponent } from './friends/friends.component';
import { FindFriendsComponent } from './find-friends/find-friends.component';
import { FriendRequestsComponent } from './friend-requests/friend-requests.component';

export const routing: ModuleWithProviders = RouterModule.forChild([
  {
    path: 'friends',
    component: RootComponent, canActivate: [AuthGuard],

    children: [
      { path: '', component:  FriendsComponent},
      { path: 'friends', component:  FriendsComponent},
      { path: 'find', component: FindFriendsComponent },
      { path: 'requests', component: FriendRequestsComponent },
    ]
  }
]);
