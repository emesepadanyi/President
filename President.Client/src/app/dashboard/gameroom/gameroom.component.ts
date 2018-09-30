import { Component, OnInit } from '@angular/core';
import { Card } from '../models/card.interface';
import { Http } from '@angular/http';
import { ConfigService } from '../../services/config.service';
import { HubConnection } from '@aspnet/signalr';
import signalR = require('@aspnet/signalr');
import { GameStatus } from '../models/game.status.interface';

@Component({
  selector: 'app-gameroom',
  templateUrl: './gameroom.component.html',
  styleUrls: ['./gameroom.component.css']
})
export class GameroomComponent implements OnInit {
  private _hubConnection: HubConnection;
  private hand : Card[];

  constructor(private http: Http, private configService: ConfigService) { }

  ngOnInit(): void {
    let authToken = localStorage.getItem('auth_token');

    this._hubConnection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:5001/gameHub", { accessTokenFactory: () => authToken })
      .build();

    this._hubConnection
      .start()
      .then(() => console.log('Connection started!'))
      .catch(err => console.log('Error while establishing connection :('));

    this._hubConnection.on('StartGame', (gameStatus: GameStatus) => {
      console.log("something happened");
      console.log(gameStatus);
      this.hand = gameStatus.cards;
    });
  }
}
