import { routing }  from './game.routing';
import { NgModule } from '@angular/core';
import { FormsModule }  from '@angular/forms';
import { CommonModule } from '@angular/common';
import { LobbyComponent } from './lobby/lobby.component';
import { AuthGuard } from '../auth.guard';

@NgModule({
  imports: [
    routing,
    CommonModule,
    FormsModule
  ],
  declarations: [LobbyComponent],
  providers: [AuthGuard]
})
export class GameModule { }
