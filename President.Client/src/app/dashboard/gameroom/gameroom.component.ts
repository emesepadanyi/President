import { Component, OnInit, OnDestroy } from '@angular/core';
import { HubConnection } from '@aspnet/signalr';
import signalR = require('@aspnet/signalr');

import { GameStatus } from '../models/game.status.interface';
import { Card }       from '../models/card.interface';
import { MoveStatus } from '../models/move.status.interface';
import { GameService } from '../services/game.service';
import { Game } from '../models/game';
import { NewRound } from '../models/new.round.interface';
import * as $ from 'jquery';


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
  private newRound: NewRound;

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
      this.switchCards = false;
      this.newRound = null;
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

    this._hubConnection.on('WaitForNewRound', (newRound: NewRound) => {
      this.switchCards = true;
      this.newRound = newRound;
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

  getCallToActionString(): string {
    if(this.newRound.wait){
      return "You will receive card(s) soon!";
    }else{
      return "Please select card(s) to swap!";
    }
  }

  sendSwitchableCards(){
    var no = ($("input.selectableCards").toArray() as Array<HTMLInputElement>).filter(card => card.checked == true);
    var selectedCards = new Array<Card>();
    no.forEach(card =>
      {
        var c = new Card();
        c.name = card.id.slice(0,1);
        var slice = 1;
        if(c.name === '1'){c.name = '10'; slice = 2;}
        c.suit = card.id.slice(slice);
        selectedCards.push(c);
      });
    this.gameService.switchCards(selectedCards)
      .subscribe(() => { });
  }

  ngOnDestroy(): void {
    this._hubConnection.off("StartGame");
    this._hubConnection.off("PutCard");
    this._hubConnection.off("ResetDeck");
    this._hubConnection.off('WaitForNewRound');
    this._hubConnection.stop();
  }
}
