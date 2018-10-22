import { Component, OnInit } from '@angular/core';
import { HubConnection } from '@aspnet/signalr';
import signalR = require('@aspnet/signalr');

import { GameStatus } from '../models/game.status.interface';
import { Hand }       from '../models/hand.interface';
import { Card }       from '../models/card.interface';
import { MoveStatus } from '../models/move.status.interface';
import { GameService } from '../services/game.service';

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

  constructor(private gameSetvice: GameService) { }

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

    this._hubConnection.on('PutCard', (moveStatus: MoveStatus) => {
      console.log(moveStatus);
      this.hand = moveStatus.cards;
      this.enemyHands = moveStatus.hands;
      this.nextUser = moveStatus.nextUser;
      this.deck.push(moveStatus.movedCard);
    });
  }

  counter(i: number){
    return new Array(i);
  }
  
  isActivePlayer(user: string): boolean{
    return this.nextUser === user;
  }

  clickedOn(card: Card) {
    this.gameSetvice.sendCard(card)
      .subscribe(() => { });
  }
}
