import { TestBed, inject } from '@angular/core/testing';

import { FriendService } from './friend.service';
import { ConfigService } from '../../services/config.service';
import { Http } from '@angular/http';
import { of } from 'rxjs/observable/of';

describe('FriendService', () => {
  beforeEach(() => {
    const httpService = jasmine.createSpyObj('Http', ['post']);
    httpService.post.and.returnValue(of(Request));

    TestBed.configureTestingModule({
      providers: [FriendService, ConfigService, { provide: Http, useValue: httpService }]
    });
  });

  it('is created', inject([FriendService], (service: FriendService) => {
    expect(service).toBeTruthy();
  }));

  it('sends a query to the backend', inject([FriendService], (service: FriendService) => {
    var query = service.sendRequest('1');
    expect(query).toBeDefined();
  }));
});
