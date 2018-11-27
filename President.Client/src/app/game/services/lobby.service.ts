import { Injectable } from '@angular/core';
import { BaseService } from '../../services/base.service';
import { Http } from '@angular/http';
import { ConfigService } from '../../services/config.service';
import { Observable } from 'rxjs';

@Injectable()
export class LobbyService extends BaseService{
  constructor(private http: Http, private configService: ConfigService) {
    super();
  }

  public getOnlineFriends(): Observable<string[]> {
    var headers = this.getHeader();

    return this.http
      .get(this.configService.getApiURI() + "/lobby", {headers})
      .map(response => response.json())
      .catch(this.handleError);
  }

  createNewRoom(friends: string[]): Observable<string> {
    var headers = this.getHeader();

    return this.http
      .post(this.configService.getApiURI() + "/game", friends, {headers})
      .map(response => true)
      .catch(this.handleError);
  }
}
