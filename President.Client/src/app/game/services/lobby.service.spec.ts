import { TestBed, inject } from '@angular/core/testing';

import { LobbyService } from './lobby.service';
import { ConfigService } from '../../services/config.service';
import { Http } from '@angular/http';
import { of } from 'rxjs/observable/of';

describe('LobbyService', () => {
  beforeEach(() => {
    const httpService = jasmine.createSpyObj('Http', ['post']);
    httpService.post.and.returnValue(of(Request));

    TestBed.configureTestingModule({
      providers: [LobbyService, ConfigService, { provide: Http, useValue: httpService }]
    });
  });

  it('should be created', inject([LobbyService], (service: LobbyService) => {
    expect(service).toBeTruthy();
  }));
});
