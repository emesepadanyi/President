import { Component, OnInit } from '@angular/core';
import { NewGame } from '../models/new-game';

@Component({
  selector: 'app-lobby',
  templateUrl: './lobby.component.html',
  styleUrls: ['./lobby.component.css']
})
export class LobbyComponent implements OnInit {
  friends: string[];
  userName: string;
  newGameForm: NewGame;

  constructor() { 
  }

  ngOnInit() {
    this.friends = ["Noncsi", "Peti", "Ede", "Bence"];
    this.userName = localStorage.getItem("user_name");

    this.newGameForm = {
      friend0: this.userName,
      friend1: this.friends[0],
      friend2: this.friends[1],
      friend3: this.friends[2],
      cards: 13,
      rounds: 10
    }
  }

  submit({value, valid}: {value: NewGame, valid: boolean}){
    this.newGameForm.friend1 = value.friend1 || this.newGameForm.friend1;
    this.newGameForm.friend2 = value.friend2 || this.newGameForm.friend2;
    this.newGameForm.friend3 = value.friend3 || this.newGameForm.friend3;
    this.newGameForm.cards = value.cards || this.newGameForm.cards;
    this.newGameForm.rounds = value.rounds || this.newGameForm.rounds;
    debugger;
  }

}
