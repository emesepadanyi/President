import { Component, OnInit } from '@angular/core';
import { HttpHeaders } from '@angular/common/http';
import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';

@Component({
    selector: 'app-chat-component',
    templateUrl: './chat.component.html',
    styleUrls: ['./chat.component.scss']
})

export class ChatComponent implements OnInit {
    private _hubConnection: HubConnection | undefined;
    public async: any;
    private headers: HttpHeaders;
    message = '';
    messages: string[] = [];

    constructor() {
      // this.headers = new HttpHeaders();
      // this.headers = this.headers.set('Content-Type', 'application/json');
      // this.headers = this.headers.set('Accept', 'application/json');
      // let authToken = localStorage.getItem('auth_token');
      // this.headers.append('Authorization', `Bearer ${authToken}`);
    }

    public sendMessage(): void {
        const data = `Sent: ${this.message}`;

        if (this._hubConnection) {
            this._hubConnection.invoke('Send', data);
        }
        this.messages.push(data);
    }

    ngOnInit() {
      let authToken = localStorage.getItem('auth_token');
      this._hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("https://localhost:5001/chat"
        //, { accessTokenFactory: () => authToken }
        )
        .build();
          
      /*this._hubConnection = new signalR.HubConnectionBuilder()
      //TODO remove this string
        .withUrl('https://localhost:5001/chat', {headers: this.headers})
        .configureLogging(signalR.LogLevel.Trace)
        .build();*/

      this._hubConnection.start().catch(err => console.error(err.toString()));

      this._hubConnection.on('Send', (data: any) => {
          const received = `Received: ${data}`;
          this.messages.push(received);
      });
  }

}
