import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FriendsComponent } from './friends.component';
import { UserCardComponent } from '../user-card/user-card.component';
import { User } from '../models/user.interface';
import { of } from 'rxjs/observable/of';
import { FriendService } from '../services/friend.service';

describe('FriendsComponent', () => {
  let component: FriendsComponent;
  let fixture: ComponentFixture<FriendsComponent>;

  beforeEach(async(() => {
    let users = new Array<User>();
    const friendService = jasmine.createSpyObj('FriendService', ['getFriends']);
    friendService.getFriends.and.returnValue(of(users));

    TestBed.configureTestingModule({
      declarations: [FriendsComponent, UserCardComponent],
      providers: [{ provide: FriendService, useValue: friendService }]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FriendsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
