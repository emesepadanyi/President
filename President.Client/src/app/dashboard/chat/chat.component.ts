import { Component, OnInit } from "@angular/core";
import { ConfigService } from "../../services/config.service";
import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';
import { Http, Headers } from "@angular/http";
import { ChatMessage } from "../models/chat.message.interface";


@Component({
  selector: 'app-chat-component',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})

export class ChatComponent implements OnInit {
  private _hubConnection: HubConnection;
  messages: ChatMessage[] = [];
  message: string;

  constructor(private http: Http, private configService: ConfigService) { }

  ngOnInit(): void {
    this._hubConnection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:5001/chat")
      .build();

    this._hubConnection
      .start()
      .then(() => console.log('Connection started!'))
      .catch(err => console.log('Error while establishing connection :('));

    this._hubConnection.on('BroadcastMessage', (chatMessage: ChatMessage) => {
      console.log(chatMessage);
      this.messages.push( chatMessage );
    });
  }

  public sendMessage() {
    let headers = new Headers();
    headers.append('Content-Type', 'text/json');
    let authToken = localStorage.getItem('auth_token');
    headers.append('Authorization', `Bearer ${authToken}`);

    this.http.post(this.configService.getApiURI() + "/message", `"${this.message}"`, {headers}) 
    .subscribe( () => {});
  }
}
