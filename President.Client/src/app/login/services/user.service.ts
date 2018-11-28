import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';

import { UserRegistration } from '../models/user.registration.interface';
import { ConfigService } from '../../services/config.service';

import {BaseService} from "../../services/base.service";

import { Observable } from 'rxjs/Rx';
import { BehaviorSubject } from 'rxjs/Rx'; 

// Add the RxJS Observable operators we need in this app.
import '../../rxjs-operators';
import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';

@Injectable()

export class UserService extends BaseService {
  baseUrl: string = '';

  private _authNavStatusSource = new BehaviorSubject<boolean>(false);
  public notification = new BehaviorSubject<boolean>(false);
  authNavStatus$ = this._authNavStatusSource.asObservable();
  _hubConnection: HubConnection;

  private loggedIn = false;

  constructor(private http: Http, private configService: ConfigService) {
    super();
    this.loggedIn = !!localStorage.getItem('auth_token');
    this._authNavStatusSource.next(this.loggedIn);
    this.baseUrl = configService.getApiURI();
  }

  register(email: string, password: string, firstName: string, lastName: string): Observable<UserRegistration> {
    let body = JSON.stringify({ email, password, firstName, lastName });
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let options = new RequestOptions({ headers: headers });

    return this.http.post(this.baseUrl + "/accounts", body, options)
      .map(res => true)
      .catch(this.handleError);
  }  

  login(userName, password) {
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');

    return this.http
      .post(
      this.baseUrl + '/auth/login',
      JSON.stringify({ userName, password }),{ headers }
      )
      .map(res => res.json())
      .map(res => {
        localStorage.setItem('auth_token', res.auth_token);
        localStorage.setItem('user_name', userName);
        this.loggedIn = true;
        this._authNavStatusSource.next(true);
        return true;
      })
      .catch(this.handleError);
  }

  subscribeOnline(){
    let authToken = localStorage.getItem('auth_token');

    this._hubConnection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:5001/online", { accessTokenFactory: () => authToken })
      .build();

    this._hubConnection
      .start()
      .then(() => console.log('Connection started!'))
      .catch(err => console.log('Error while establishing connection :('));

    this._hubConnection.on('Invite', () => {
      console.log('invitation');
      this.notification.next(true);
    });
  }

  logout() {
    this._hubConnection.off('Invite');
    this._hubConnection.stop();
    localStorage.removeItem('auth_token');
    localStorage.removeItem('user_name');
    this.loggedIn = false;
    this._authNavStatusSource.next(false);
  }

  isLoggedIn() {
    if(this.isLoggedIn){
      if(!this._hubConnection){
        this.subscribeOnline();
      }
    }
    return this.loggedIn;
  }
}
