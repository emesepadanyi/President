import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HeaderComponent } from './header.component';
import { UserService } from '../login/services/user.service';
import { Observable } from 'rxjs';
import { of } from 'rxjs/observable/of';

describe('HeaderComponent', () => {
  let component: HeaderComponent;
  let fixture: ComponentFixture<HeaderComponent>;

  beforeEach(async(() => {
    let userService = jasmine.createSpyObj('UserService', ['']);
    userService.authNavStatus$ = of(true);
    
    TestBed.configureTestingModule({
      declarations: [ HeaderComponent ]
      , providers: [
        { provide: UserService, useValue: userService }
      ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
