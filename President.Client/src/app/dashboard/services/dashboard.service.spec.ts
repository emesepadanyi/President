import { TestBed, inject } from '@angular/core/testing';

import { DashboardService } from './dashboard.service';
import { ConfigService } from '../../services/config.service';
import { Http } from '@angular/http';
import { of } from 'rxjs/observable/of';

describe('DashboardService', () => {
  beforeEach(() => {
    const httpService = jasmine.createSpyObj('Http', ['post']);
    httpService.post.and.returnValue(of(Request));

    TestBed.configureTestingModule({
      providers: [DashboardService, ConfigService, { provide: Http, useValue: httpService }]
    });
  });

  it('should be created', inject([DashboardService], (service: DashboardService) => {
    expect(service).toBeTruthy();
  }));
});
