import { Component, OnInit, OnDestroy } from '@angular/core';
import { HubConnection } from '@aspnet/signalr';
import signalR = require('@aspnet/signalr');

import { GameStatus } from '../models/game.status.interface';
import { Card }       from '../models/card.interface';
import { MoveStatus } from '../models/move.status.interface';
import { GameService } from '../services/game.service';
import { Game } from '../models/game';

@Component({
  selector: 'app-gameroom',
  templateUrl: './gameroom.component.html',
  styleUrls: ['./gameroom.component.scss', './card.css']
})
export class GameroomComponent implements OnInit, OnDestroy {
  private _hubConnection: HubConnection;
  private user: string;
  private game: Game;
  private switchCards: boolean = false;

  constructor(private gameService: GameService) { }

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
      if(!this.game) this.game = new Game();
      this.game.setUp(gameStatus);
    });

    this._hubConnection.on('PutCard', (moveStatus: MoveStatus) => {
      console.log(moveStatus);
      this.game.moveCard(moveStatus);
    });

    this._hubConnection.on('ResetDeck', (nextUser: string) => {
      console.log("reset deck", nextUser);
      this.game.nextUser = nextUser;
      this.game.resetDeck();
    });
  }

  counter(i: number){
    return new Array(i);
  }
  
  isActivePlayer(user: string): boolean{
    return this.game.nextUser === user;
  }

  clickedOn(card: Card) {
    this.gameService.sendCard(card)
      .subscribe(() => { });
  }

  pass(): void{
    this.gameService.pass()
      .subscribe(() => { });
  }

  ngOnDestroy(): void {
    this._hubConnection.off("StartGame");
    this._hubConnection.off("PutCard");
    this._hubConnection.off("ResetDeck");
    this._hubConnection.stop();
  }
}
