import { Component, Inject, Input, ViewContainerRef, ViewEncapsulation } from "@angular/core";
import { finalize } from "rxjs/operators";
import { CellCaptionPipe } from "../cell/cell-caption.pipe";
import { ErrorService } from "../errors/error.service";
import { GameService } from "../game.service";
import { GameClient, BASE_API_URL } from "../generated/clients";
import { Cell, CellType, GameModel } from "../generated/dto";
import { UserService } from "../user.service";
import * as signalR from "@microsoft/signalr";

@Component({
    selector: 't-my-game',
    templateUrl: './my-game.component.html',
    styleUrls: ['./my-game.component.css'],
    encapsulation: ViewEncapsulation.ShadowDom
})
export class MyGameComponent {

    private _baseApiUrl: string;
    private _game: GameModel;

    constructor(
        private userService: UserService,
        private gameClient: GameClient,
        private gameService: GameService,
        private cellCaptionPipe: CellCaptionPipe,
        private errorService: ErrorService,
        @Inject(BASE_API_URL) baseApiUrl: string,
        private viewContainer: ViewContainerRef
    ) {
        this._baseApiUrl = baseApiUrl;
    }

    StyleSheets: Node[];
    IsLoading = false;
    IsBoardInteractive = true;
    Note: string;

    get Game(): GameModel {
        return this._game;
    }

    @Input() set Game(value: GameModel) {
        this._game = value;

        const playerCellTypes: CellType[] = [];
        if (value.crossPlayerId == this.userService.GetUserId()) {
            playerCellTypes.push(CellType.Cross)
        }
        if (value.zeroPlayerId == this.userService.GetUserId()) {
            playerCellTypes.push(CellType.Zero)
        }

        if (this.Game.board.winner !== CellType.None) {
            this.Note = playerCellTypes.includes(this.Game.board.winner) ? 'Вы победили' : `Победил оппонент ${this.cellCaptionPipe.transform(this.Game.board.winner)}`;
            this.IsBoardInteractive = false;
        } else {
            if (this.Game.board.nextTurn === CellType.None) {
                this.Note = "Ничья.";
                this.IsBoardInteractive = false;
            } else {
                this.Note = `${playerCellTypes.includes(this.Game.board.nextTurn) ? 'Ваш ход' : 'Ход оппонента'} ${this.cellCaptionPipe.transform(this.Game.board.nextTurn)}`;
                this.setupSignalRConnection();
            }
        }
    }

    public ngOnInit() {
        this.StyleSheets = Array.from(this.viewContainer.element.nativeElement.shadowRoot.querySelectorAll('style'));
    }

    public cellClicked(cell: Cell) {
        this.gameClient.turn(this.Game.id, this.userService.GetUserId(), cell.number)
            .pipe(finalize(() => {
                this.updateGame();
            }))
            .subscribe(() => { }, error => this.handleError(error));
    }

    private updateGame() {
        this.IsLoading = true;

        this.gameClient.get(this.Game.id)
            .pipe(finalize(() => {
                this.IsLoading = false;
            }))
            .subscribe(game => this.Game = game, error => this.handleError(error));
    }

    private async setupSignalRConnection(): Promise<void> {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl(`${this._baseApiUrl}/game-hub`)
            .withAutomaticReconnect()
            .build();

        connection.onreconnecting(err => {
            this.IsBoardInteractive = false;
        });

        connection.onreconnected(err => {
            this.IsBoardInteractive = true;
        });

        connection.on('turn', (gameId) => {
            this.updateGame();
        });

        try {
            return await connection.start();
        } catch (err) {
            alert(`WebSocket connection.start error: ${err}`);
        }
    }

    private handleError(error: any) {
        alert(this.errorService.Parse(error));
    }
}