import { Component, Input, ViewEncapsulation } from '@angular/core';
import { GameClient } from '../generated/clients';
import { CellType, GameModel } from '../generated/dto';

@Component({
    selector: 't-free-game',
    templateUrl: './free-game.component.html',
    styleUrls: ['./free-game.component.css'],
    encapsulation: ViewEncapsulation.ShadowDom
})
export class FreeGameComponent {

    PlayerCrossName = 'Игрок 1';
    PlayerZeroName = 'Игрок 2';
    JoinText = 'Присоединиться';

    @Input() Game: GameModel;
    CellType = CellType;

    constructor(
        private gameClient: GameClient
    ) { }

    public ngOnInit() { }

    public joinButtonClick(cellType: CellType) {
        switch (cellType) {
            case CellType.Cross:
                this.gameClient.setCrossPlayer(this.Game.id)
                    .subscribe(x => {
                        this.updateGame();
                    }, error => {
                        debugger;
                    });
                return;
            case CellType.Zero:
                this.gameClient.setZeroPlayer(this.Game.id)
                    .subscribe(x => {
                        this.updateGame();
                    }, error => {
                        debugger;
                    });
                return;
        }
    }

    private updateGame() {
        this.gameClient.get(this.Game.id).subscribe(game => this.Game = game);
    }
}