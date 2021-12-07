import { Component } from '@angular/core';
import { GameClient } from '../generated/clients';
import { GameModel } from '../generated/dto';

@Component({
    selector: 't-free-games',
    templateUrl: './free-games.component.html',
    styleUrls: ['./free-games.component.css']
})
export class FreeGamesComponent {

    public Games: GameModel[];

    constructor(
        private gameClient: GameClient
    ) { }

    ngOnInit() {
        this.gameClient.getFree().subscribe(games => {
            this.Games = games;
        });
    }
}