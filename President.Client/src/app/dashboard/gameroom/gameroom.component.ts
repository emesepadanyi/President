import { Component, OnInit } from '@angular/core';
import { Card } from '../models/card.interface';
import { Http } from '@angular/http';
import { ConfigService } from '../../services/config.service';
import { HubConnection } from '@aspnet/signalr';
import signalR = require('@aspnet/signalr');
import { GameStatus } from '../models/game.status.interface';
import { Hand } from '../models/hand.interface';

@Component({
  selector: 'app-gameroom',
  templateUrl: './gameroom.component.html',
  styleUrls: ['./gameroom.component.scss', './card.css']
})
export class GameroomComponent implements OnInit {
  private _hubConnection: HubConnection;
  private hand : Card[];
  private deck : Card[] = new Array<Card>();
  private enemyHands: Hand[];
  private nextUser: string;
  private user: string;

  constructor(private http: Http, private configService: ConfigService) { }

  ngOnInit(): void {
    let authToken = localStorage.getItem('auth_token');
    this.user = localStorage.getItem('user_name');

    this._hubConnection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:5001/gameHub", { accessTokenFactory: () => authToken })
      .build();

    this._hubConnection
      .start()
      .then(() => console.log('Connection started!'))
      .catch(err => console.log('Error while establishing connection :('));

    this._hubConnection.on('StartGame', (gameStatus: GameStatus) => {
      this.hand = gameStatus.cards;
      this.enemyHands = gameStatus.hands;
      this.nextUser = gameStatus.nextUser;
    });
  }

  counter(i: number){
    return new Array(i);
  }
  
  isActivePlayer(user: string): boolean{
    return this.nextUser === user;
  }

  clickedOn(card: Card){
    console.log(card);

    const index = this.hand.indexOf(card, 0);
    if (index > -1) {
      this.hand.splice(index, 1);
    }

    this.deck.push(card);
  }
}
