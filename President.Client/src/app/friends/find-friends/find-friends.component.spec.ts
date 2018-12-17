import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FindFriendsComponent } from './find-friends.component';
import { FormsModule } from '@angular/forms';
import { UserCardComponent } from '../user-card/user-card.component';
import { User } from '../models/user.interface';
import { of } from 'rxjs/observable/of';
import { FriendService } from '../services/friend.service';

describe('FindFriendsComponent', () => {
  let component: FindFriendsComponent;
  let fixture: ComponentFixture<FindFriendsComponent>;

  beforeEach(async(() => {
    let users = new Array<User>();
    const friendService = jasmine.createSpyObj('FriendService', ['findFriends']);
    friendService.findFriends.and.returnValue(of(users));

    TestBed.configureTestingModule({
      declarations: [ FindFriendsComponent, UserCardComponent ]
      , imports: [FormsModule]
      , providers: [{ provide: FriendService, useValue: friendService }]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FindFriendsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
