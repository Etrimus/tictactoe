import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { GameListComponent } from "./game-list/game-list.component";

const gameRoutes: Routes = [
    { path: 'game', component: GameListComponent }
]

@NgModule({
    imports: [RouterModule.forChild(gameRoutes)],
    exports: [RouterModule]
})
export class GameRoutingModule { }