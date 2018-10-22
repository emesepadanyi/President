import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../modules/shared.module';

import { RootComponent } from './root/root.component';
import { ChatComponent } from './chat/chat.component';
import { GameroomComponent } from './gameroom/gameroom.component';
import { HomeComponent } from './home/home.component';
import { SettingsComponent } from './settings/settings.component';

import { AuthGuard } from '../auth.guard';
import { DashboardService } from './services/dashboard.service';
import { routing }  from './dashboard.routing';
import { GameService } from './services/game.service';


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    routing,
    SharedModule
  ],
  declarations: [RootComponent,HomeComponent, SettingsComponent, ChatComponent, GameroomComponent],
  exports:      [ ],
  providers:    [AuthGuard, DashboardService, GameService]
})
export class DashboardModule { }