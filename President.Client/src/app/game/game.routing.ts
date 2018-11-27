import { ModuleWithProviders } from "@angular/core";
import { RouterModule } from "@angular/router";
import { AuthGuard } from "../auth.guard";
import { LobbyComponent } from "./lobby/lobby.component";
import { GameroomComponent } from "./gameroom/gameroom.component";

export const routing: ModuleWithProviders = RouterModule.forChild([
  {
    path: 'game',
    //component: RootComponent, 
    canActivate: [AuthGuard],

    children: [
      { path: '', component: LobbyComponent },
      { path: 'lobby', component: LobbyComponent },
      { path: 'gameroom', component: GameroomComponent },
    ]
  }
]);
