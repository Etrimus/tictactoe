import { Component, Input, ViewContainerRef, ViewEncapsulation } from "@angular/core";
import { ErrorService } from "../errors/error.service";
import { GameService } from "../game.service";
import { GameClient } from "../generated/clients";
import { CellType, GameModel } from "../generated/dto";
import { UserService } from "../user.service";

@Component({
    selector: 't-my-game',
    templateUrl: './my-game.component.html',
    styleUrls: ['./my-game.component.css'],
    encapsulation: ViewEncapsulation.ShadowDom
})
export class MyGameComponent {

    private _game: GameModel;

    constructor(
        private userService: UserService,
        private gameClient: GameClient,
        private gameService: GameService,
        private errorService: ErrorService,
        private viewContainer: ViewContainerRef
    ) { }

    StyleSheets: Node[];
    IsLoading = false;
    Note: string;

    get Game(): GameModel {
        return this._game;
    }

    @Input() set Game(value: GameModel) {
        this._game = value;

        const playerCellType = value.crossPlayerId === this.userService.GetUserId()
            ? CellType.Cross
            : value.zeroPlayerId == this.userService.GetUserId()
                ? CellType.Zero
                : CellType.None;

        this.Note = this.Game.board.nextTurn === playerCellType
            ? "Ваш ход!"
            : "Ход оппонента."
    }

    public ngOnInit() {
        this.StyleSheets = Array.from(this.viewContainer.element.nativeElement.shadowRoot.querySelectorAll('style'));
    }

}