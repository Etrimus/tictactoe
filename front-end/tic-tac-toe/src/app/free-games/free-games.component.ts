import { Component, ViewEncapsulation } from '@angular/core';
import { GameClient } from '../generated/clients';
import { GameModel } from '../generated/dto';

@Component({
    selector: 't-free-games',
    templateUrl: './free-games.component.html',
    styleUrls: ['./free-games.component.css'],
    encapsulation: ViewEncapsulation.ShadowDom
})
export class FreeGamesComponent {

    public Games: GameModel[] = [];

    public IsAnyGames = true;

    constructor(private gameClient: GameClient) { }

    ngOnInit() {
        this.gameClient.getFree().subscribe(games => {
            this.Games = games;
            this.IsAnyGames = this.Games && this.Games.length > 0;
        });
    }
}