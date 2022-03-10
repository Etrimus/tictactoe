import { Component, EventEmitter, Inject, Input, Output } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { finalize } from 'rxjs/operators';
import { ErrorService } from '../errors/error.service';
import { GameService } from '../game.service';
import { BASE_API_URL, GameClient } from '../generated/clients';
import { CellType, GameModel } from '../generated/dto';
import { UserService } from '../user.service';

@Component({
    selector: 't-free-game',
    templateUrl: './free-game.component.html',
    styleUrls: ['./free-game.component.css']
})
export class FreeGameComponent {

    private _baseApiUrl: string;
    private _hubConnection: signalR.HubConnection;

    public JoinText = 'Присоединиться';
    public IsLoading = false;

    @Input() public Game: GameModel;
    @Output() public GameChange = new EventEmitter<GameModel>();

    public CellType = CellType;
    public StyleSheets: Node[];

    constructor(
        private userService: UserService,
        private gameClient: GameClient,
        private gameService: GameService,
        private errorService: ErrorService,
        @Inject(BASE_API_URL) baseApiUrl: string
    ) {
        this._baseApiUrl = baseApiUrl;
    }

    public ngOnInit() {
        this.setupSignalRConnection();
    }

    public async ngOnDestroy() {
        await this.stopSignalRConnection();
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

    private async setupSignalRConnection(): Promise<void> {
        this._hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(`${this._baseApiUrl}/game-hub`)
            .withAutomaticReconnect()
            .build();

        this._hubConnection.onreconnecting(err => { });

        this._hubConnection.onreconnected(err => { });

        this._hubConnection.on('game-updated', (gameId) => {
            if (this.Game.id === gameId) {
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

    private updateGame() {
        this.IsLoading = true;

        this.gameClient.get(this.Game.id)
            .pipe(finalize(() => this.IsLoading = false))
            .subscribe(game => { this.Game = game; this.GameChange.emit(game) }, error => this.handleError(error));
    }

    private handleError(error: any) {
        alert(this.errorService.Parse(error));
    }
}
