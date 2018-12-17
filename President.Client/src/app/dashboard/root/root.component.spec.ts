import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RootComponent } from './root.component';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

describe('RootComponent', () => {
  let component: RootComponent;
  let fixture: ComponentFixture<RootComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RootComponent ]
      , imports: [
        FormsModule
        , RouterTestingModule
      ]
      , providers: [
        { provide: Router, useClass: class { navigate = jasmine.createSpy("navigate"); } }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    debugger;
    fixture = TestBed.createComponent(RootComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });
});
