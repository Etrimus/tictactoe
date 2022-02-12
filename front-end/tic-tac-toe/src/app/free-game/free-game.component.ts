import { Component, Input, ViewContainerRef, ViewEncapsulation } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { ErrorService } from '../errors/error.service';
import { GameService } from '../game.service';
import { GameClient } from '../generated/clients';
import { CellType, GameModel } from '../generated/dto';
import { UserService } from '../user.service';

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
    IsLoading = false;

    @Input() Game: GameModel;
    CellType = CellType;
    StyleSheets: Node[];

    constructor(
        private userService: UserService,
        private gameClient: GameClient,
        private gameService: GameService,
        private errorService: ErrorService,
        private viewContainer: ViewContainerRef
    ) { }

    public ngOnInit() {
        this.StyleSheets = Array.from(this.viewContainer.element.nativeElement.shadowRoot.querySelectorAll('style'));
    }

    public joinButtonClick(cellType: CellType) {
        this.IsLoading = true;
        switch (cellType) {
            case CellType.Cross:
                this.gameService.SetCrossPlayer(this.Game.id, this.userService.GetUserId())
                    .pipe(finalize(() => this.updateGame()))
                    .subscribe(() => { }, error => this.handleError(error));
                return;
            case CellType.Zero:
                this.gameService.SetZeroPlayer(this.Game.id, this.userService.GetUserId())
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