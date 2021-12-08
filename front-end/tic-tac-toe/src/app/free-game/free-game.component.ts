import { Component, Input } from '@angular/core';
import { GameClient } from '../generated/clients';
import { GameModel } from '../generated/dto';

@Component({
    selector: 't-free-game',
    templateUrl: './free-game.component.html',
    styleUrls: ['./free-game.component.css']
})
export class FreeGameComponent {

    @Input() Game: GameModel;

    constructor(
        private gameClient: GameClient
    ) { }

    ngOnInit() { }
}