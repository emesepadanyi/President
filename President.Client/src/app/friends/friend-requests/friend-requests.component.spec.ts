import { async, ComponentFixture, TestBed, tick } from '@angular/core/testing';

import { FriendRequestsComponent } from './friend-requests.component';
import { FriendService } from '../services/friend.service';
import { UserCardComponent } from '../user-card/user-card.component';
import { User } from '../models/user.interface';
import { of } from 'rxjs/observable/of';

describe('FriendRequestsComponent', () => {
  let component: FriendRequestsComponent;
  let fixture: ComponentFixture<FriendRequestsComponent>;
  let users: Array<User>;

  beforeEach(async(() => {
    users = new Array<User>();
    const friendService = jasmine.createSpyObj('FriendService', ['getRequests']);
    friendService.getRequests.and.returnValue(of(users));

    TestBed.configureTestingModule({
      declarations: [FriendRequestsComponent, UserCardComponent],
      providers: [{ provide: FriendService, useValue: friendService }]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FriendRequestsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('is created', () => {
    expect(component).toBeDefined();
  });

  it('shows message', () => {
    let el = fixture.nativeElement.querySelector('h2');
    expect(el.innerText).toContain("Nothing to show. You've responded to all requests!");
  });

  it('shows one user', () => {
    users.push({ id: '1', userName: 'test1', pictureUrl: '' });
    fixture.detectChanges();
    let el = fixture.nativeElement.querySelectorAll('app-user-card');
    expect(el.length).toEqual(1);
  });

  it('shows two users', () => {
    users.push({ id: '1', userName: 'test1', pictureUrl: '' });
    users.push({ id: '2', userName: 'test2', pictureUrl: '' });
    fixture.detectChanges();
    let el = fixture.nativeElement.querySelectorAll('app-user-card');
    expect(el.length).toEqual(2);
  });
});
