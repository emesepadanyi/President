import { TestBed, inject } from '@angular/core/testing';

import { GameService } from './game.service';
import { of } from 'rxjs/observable/of';
import { ConfigService } from '../../services/config.service';
import { Http } from '@angular/http';

describe('GameService', () => {
  beforeEach(() => {
    const httpService = jasmine.createSpyObj('Http', ['post']);
    httpService.post.and.returnValue(of(Request));

    TestBed.configureTestingModule({
      providers: [GameService, ConfigService, { provide: Http, useValue: httpService }]
    });
  });

  it('should be created', inject([GameService], (service: GameService) => {
    expect(service).toBeTruthy();
  }));
});
