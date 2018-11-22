import { Injectable } from '@angular/core';
import { BaseService } from '../../services/base.service';
import { Http, Headers, Response } from '@angular/http';
import { ConfigService } from '../../services/config.service';
import { User } from '../models/user.interface';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class FriendService extends BaseService {
  baseUrl: string = '';

  constructor(private http: Http, private configService: ConfigService) {
    super();
    this.baseUrl = configService.getApiURI();
  }

  private getHeader(): Headers{
    let headers = new Headers();
    headers.append('Content-Type', 'text/json');
    let authToken = localStorage.getItem('auth_token');
    headers.append('Authorization', `Bearer ${authToken}`);
    return headers;
  }

  public getFriends(): Observable<User[]> {
    var headers = this.getHeader();

    return this.http
      .get(this.configService.getApiURI() + "/friends", {headers})
      .map(response => response.json())
      .catch(this.handleError);
  }

  public getRequests(): Observable<User[]> {
    var headers = this.getHeader();

    return this.http
      .get(this.configService.getApiURI() + "/requests", {headers})
      .map(response => response.json())
      .catch(this.handleError);
  }

  public acceptRequest(senderId: string){
    var headers = this.getHeader();

    return this.http
      .put(this.configService.getApiURI() + "/requests/accept", JSON.stringify({id: senderId}), {headers})
      .catch(this.handleError);
  }

  public rejectRequest(senderId: string){
    var headers = this.getHeader();

    return this.http
      .put(this.configService.getApiURI() + "/requests/reject", JSON.stringify({id: senderId}), {headers})
      .catch(this.handleError);
  }

  public findUsers(key: string): Observable<User[]>{
    var headers = this.getHeader();

    return this.http
      .get(this.configService.getApiURI() + "/friends/" + key, {headers})
      .map(response => response.json())
      .catch(this.handleError);
  }

  public sendRequest(userId: string){
    console.log("about to make friendship");
    let headers =  this.getHeader();

    return this.http
      .post(this.configService.getApiURI() + "/requests", JSON.stringify({id: userId}), {headers})
      .catch(this.handleError);
  }
}
