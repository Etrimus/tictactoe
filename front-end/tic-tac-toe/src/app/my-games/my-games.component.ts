import { Component } from '@angular/core';
import { GameClient } from '../generated/clients';
import { GameModel } from '../generated/dto';

@Component({
    selector: 't-my-games',
    templateUrl: './my-games.component.html',
    styleUrls: ['./my-games.component.css']
})
export class MyGamesComponent {
    public Games: GameModel[];

    constructor(
        private gameClient: GameClient
    ) { }

    ngOnInit() {
        this.gameClient.getById(['3D225EFE-AD5B-4675-BFDB-CB6E30283EF1', '3D225EFE-AD5B-4675-BFDB-CB6E30283EF1']).subscribe(games => {
            this.Games = games;
        });
    }
}