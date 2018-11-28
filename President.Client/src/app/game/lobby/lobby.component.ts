import { Component, OnInit } from '@angular/core';
import { NewGame } from '../models/new-game';
import { LobbyService } from '../services/lobby.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-lobby',
  templateUrl: './lobby.component.html',
  styleUrls: ['./lobby.component.css']
})
export class LobbyComponent implements OnInit {
  friends: string[];
  userName: string;
  error: string;
  validationError: string;
  newGameForm: NewGame;
  loading: boolean;

  constructor(private lobbyService: LobbyService, private router: Router) {
  }

  ngOnInit() {
    this.loading = true;
    this.lobbyService.getOnlineFriends()
      .subscribe(
        result => {
          if (result) {
            this.friends = result;
            this.loading = false;
          }
        },
        error => this.error = error);

    this.userName = localStorage.getItem("user_name");
  }

  submit({ value, valid }: { value: NewGame, valid: boolean }) {
    let friends = this.collectFriends(value);
    if (!this.validate(valid, friends)) {
      return
    }

    this.lobbyService.createNewRoom(friends)
      .subscribe(
        result => {
          if (result) {
            this.router.navigate(['/game/gameroom']);
          }
        },
        error => this.error = error);
  }

  validate(valid: boolean, friends: string[]): boolean {
    if(!valid){
      this.validationError = "Each player should be defined";
      return false;
    }

    if(!this.validatePlayers(friends)){
      this.validationError = "Each player should be unique";
      return false;
    }
    this.validationError = null;
    return true;
  }

  collectFriends(value: NewGame): string[] {
    let friends = new Array<string>();
    friends.push(value.friend0);
    friends.push(value.friend1);
    friends.push(value.friend2);
    friends.push(value.friend3);
    return friends;
  }

  validatePlayers(friends: string[]): boolean {
    let reducedFriends = new Set(friends);
    return reducedFriends.size == friends.length;
  }
}
