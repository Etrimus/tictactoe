import { Component, Input, ViewEncapsulation } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { ErrorService } from '../errors/error.service';
import { GameClient } from '../generated/clients';
import { CellType, GameModel } from '../generated/dto';
import { ItemRectangleComponent } from './item-rectangle/item-rectangle.component';

@Component({
    selector: 't-free-game',
    templateUrl: './free-game.component.html',
    styleUrls: ['./free-game.component.css'],
    encapsulation: ViewEncapsulation.ShadowDom
})
export class FreeGameComponent extends ItemRectangleComponent {

    PlayerCrossName = 'Игрок 1';
    PlayerZeroName = 'Игрок 2';
    JoinText = 'Присоединиться';

    @Input() Game: GameModel;
    CellType = CellType;

    constructor(
        private gameClient: GameClient,
        private errorService: ErrorService
    ) {
        super();
    }

    public ngOnInit() { }

    public joinButtonClick(cellType: CellType) {
        this.IsLoading = true;
        switch (cellType) {
            case CellType.Cross:
                this.gameClient.setCrossPlayer(this.Game.id)
                    .pipe(finalize(() => this.updateGame()))
                    .subscribe(() => { }, error => this.handleError(error));
                return;
            case CellType.Zero:
                this.gameClient.setZeroPlayer(this.Game.id)
                    .pipe(finalize(() => this.updateGame()))
                    .subscribe(() => { }, error => this.handleError(error));
                return;
        }
    }

    private updateGame() {
        this.IsLoading = true;

        this.gameClient.get(this.Game.id)
            .pipe(finalize(() => this.IsLoading = false))
            .subscribe(game => this.Game = game, error => this.handleError(error));
    }

    private handleError(error: any) {
        alert(this.errorService.Parse(error));
    }
}