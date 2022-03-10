import { Component, Inject, Input } from "@angular/core";
import { finalize } from "rxjs/operators";
import { ErrorService } from "../errors/error.service";
import { GameService } from "../game.service";
import { GameClient, BASE_API_URL } from "../generated/clients";
import { Cell, GameModel } from "../generated/dto";
import { UserService } from "../user.service";
import * as signalR from "@microsoft/signalr";
import { GameState } from "../game-state";

@Component({
    selector: 't-my-game',
    templateUrl: './my-game.component.html',
    styleUrls: ['./my-game.component.css']
})
export class MyGameComponent {

    private _baseApiUrl: string;
    private _hubConnection: signalR.HubConnection;
    private _game: GameModel;

    constructor(
        private userService: UserService,
        private gameClient: GameClient,
        private gameService: GameService,
        private errorService: ErrorService,
        @Inject(BASE_API_URL) baseApiUrl: string
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

        const gameState = this.gameService.GetState(value);

        switch (gameState) {
            case GameState.YouWin:
                this.Note = 'Вы победили';
                this.IsBoardInteractive = false;
                break;
            case GameState.YouLose:
                this.Note = 'Вы проиграли';
                this.IsBoardInteractive = false;
                break;
            case GameState.Draw:
                this.Note = 'Ничья';
                this.IsBoardInteractive = false;
                break;
            case GameState.YourTurn:
                this.Note = 'Ваш ход';
                this.IsBoardInteractive = true;
                break;
            case GameState.OpponentTurn:
                this.Note = 'Ход оппонента';
                this.IsBoardInteractive = false;
                break;
        }
    }

    public ngOnInit() {
        this.setupSignalRConnection();
    }

    public async ngOnDestroy() {
        await this.stopSignalRConnection();
    }

    public cellClicked(cell: Cell) {
        this.gameClient.turn(this.Game.id, this.userService.GetUserId(), cell.number)
            // .pipe(finalize(() => {
            //     this.updateGame();
            // }))
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
        this._hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(`${this._baseApiUrl}/game-hub`)
            .withAutomaticReconnect()
            .build();

        this._hubConnection.onreconnecting(err => {
            this.IsBoardInteractive = false;
        });

        this._hubConnection.onreconnected(err => {
            this.IsBoardInteractive = true;
        });

        this._hubConnection.on('game-updated', (gameId) => {
            if (this._game.id === gameId) {
                this.updateGame();
            }
        });

        try {
            return await this._hubConnection.start();
        } catch (err) {
            alert(`WebSocket connection.start error: ${err}`);
        }
    }

    private stopSignalRConnection(): Promise<void> {
        return this._hubConnection.stop();
    }

    private handleError(error: any) {
        alert(this.errorService.Parse(error));
    }
}