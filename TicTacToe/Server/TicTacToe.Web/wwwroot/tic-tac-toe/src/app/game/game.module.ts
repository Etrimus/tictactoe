import { NgModule } from '@angular/core';

import { GameListComponent } from './game-list/game-list.component';
import { GameRoutingModule } from './game-routing.module';

@NgModule({
    imports: [GameRoutingModule],
    declarations: [GameListComponent]
})
export class GameModule { }