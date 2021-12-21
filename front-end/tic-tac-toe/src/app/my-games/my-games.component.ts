import { Component, ViewEncapsulation } from '@angular/core';
import { GameService } from '../game.service';
import { GameClient } from '../generated/clients';
import { GameModel } from '../generated/dto';

@Component({
    selector: 't-my-games',
    templateUrl: './my-games.component.html',
    styleUrls: ['./my-games.component.css'],
    encapsulation: ViewEncapsulation.ShadowDom
})
export class MyGamesComponent {

    public IsAnyGames = false;

    public Games: GameModel[];

    constructor(
        private gameClient: GameClient,
        private gameService: GameService
    ) { }

    ngOnInit() {
        this.gameService.GetMy().subscribe(games => {
            this.Games = games;
            this.IsAnyGames = games.length > 0;
        });
    }
}