import { Component, Input, ViewEncapsulation } from '@angular/core';
import { GameClient } from '../generated/clients';
import { GameModel } from '../generated/dto';

@Component({
    selector: 't-free-game',
    templateUrl: './free-game.component.html',
    styleUrls: ['./free-game.component.css'],
    encapsulation: ViewEncapsulation.ShadowDom
})
export class FreeGameComponent {

    @Input() Game: GameModel;

    IsCrossFree: boolean;
    IsZeroFree: boolean;

    constructor(
        private gameClient: GameClient
    ) { }

    ngOnInit() { }
}