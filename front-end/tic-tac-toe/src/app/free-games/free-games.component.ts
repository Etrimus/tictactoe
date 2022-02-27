import { Component, Inject, ViewEncapsulation } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { ErrorService } from '../errors/error.service';
import { GameService } from '../game.service';
import { BASE_API_URL, GameClient } from '../generated/clients';
import { GameModel } from '../generated/dto';
import * as signalR from "@microsoft/signalr";

@Component({
    selector: 't-free-games',
    templateUrl: './free-games.component.html',
    styleUrls: ['./free-games.component.css'],
    encapsulation: ViewEncapsulation.ShadowDom
})
export class FreeGamesComponent {

    private _hubConnection: signalR.HubConnection;
    private _baseApiUrl: string;

    public IsLoading = false;
    public Games: GameModel[] = [];
    public IsAnyGames = true;

    constructor(
        private errorService: ErrorService,
        private gameClient: GameClient,
        private gameService: GameService,
        @Inject(BASE_API_URL) baseApiUrl: string,
    ) {
        this._baseApiUrl = baseApiUrl;
    }

    public async ngOnInit() {
        this.updateGames();
        await this.setupSignalRConnection();
    }

    public async ngOnDestroy() {
        await this.stopSignalRConnection();
    }

    public addGameButtonClick() {
        this.IsLoading = true;
        this.gameService.Add()
            // .pipe(finalize(() => this.updateGames()))
            .subscribe(_ => { }, error => this.handleError(error));
    }

    private async setupSignalRConnection(): Promise<void> {
        this._hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(`${this._baseApiUrl}/game-hub`)
            .withAutomaticReconnect()
            .build();

        this._hubConnection.onreconnecting(err => { });

        this._hubConnection.onreconnected(err => { });

        this._hubConnection.on('game-added', (gameId) => {
            this.updateGames();
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

    private updateGames() {
        this.IsLoading = true;
        this.gameClient.getFree().subscribe(games => {
            this.Games = games;
            this.IsAnyGames = this.Games && this.Games.length > 0;
        }, error => this.handleError(error));
    }

    private handleError(error: any) {
        alert(this.errorService.Parse(error));
    }
}