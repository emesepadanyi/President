import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeComponent } from './home.component';
import { DashboardService } from '../services/dashboard.service';
import { of } from 'rxjs/observable/of';

describe('HomeComponent', () => {
  let component: HomeComponent;
  let fixture: ComponentFixture<HomeComponent>;

  beforeEach(async(() => {
    const dashboardService = jasmine.createSpyObj('DashboardService', ['getHomeDetails']);
    dashboardService.getHomeDetails.and.returnValue(of(Request));

    TestBed.configureTestingModule({
      declarations: [ HomeComponent ]
      , providers: [
        { provide: DashboardService, useValue: dashboardService }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
