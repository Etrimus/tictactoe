import { Component, ViewEncapsulation } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { ErrorService } from '../errors/error.service';
import { GameService } from '../game.service';
import { GameClient } from '../generated/clients';
import { GameModel } from '../generated/dto';

@Component({
    selector: 't-free-games',
    templateUrl: './free-games.component.html',
    styleUrls: ['./free-games.component.css'],
    encapsulation: ViewEncapsulation.ShadowDom
})
export class FreeGamesComponent {

    IsLoading = false;
    public Games: GameModel[] = [];
    public IsAnyGames = true;

    constructor(
        private errorService: ErrorService,
        private gameClient: GameClient,
        private gameService: GameService
    ) { }

    ngOnInit() {
        this.updateGames();
    }

    public addGameButtonClick() {
        this.IsLoading = true;
        this.gameService.Add()
            .pipe(finalize(() => this.updateGames()))
            .subscribe(_ => { }, error => this.handleError(error));
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