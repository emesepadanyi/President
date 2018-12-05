import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';

import { HomeDetails } from '../models/home.details.interface';
import { ConfigService } from '../../services/config.service';

import { BaseService } from '../../services/base.service';

import { Observable } from 'rxjs/Rx';

// Add the RxJS Observable operators we need in this app.
import '../../rxjs-operators';
import { PlayerStatistics } from '../models/player.statistics.interface';

@Injectable()

export class DashboardService extends BaseService {

  constructor(private http: Http, private configService: ConfigService) {
    super();
  }

  getHomeDetails(): Observable<PlayerStatistics> {
    let headers = this.getHeader();

    return this.http.get(this.configService.getApiURI() + "/dashboard/home", { headers })
      .map(response => response.json())
      .catch(this.handleError);
  }
}