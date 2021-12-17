import { Component, ViewEncapsulation } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { ErrorService } from '../errors/error.service';
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
        private gameClient: GameClient
    ) { }

    ngOnInit() {
        this.updateGames();
    }

    public addGameButtonClick() {
        this.IsLoading = true;
        this.gameClient.add()
            .pipe(finalize(() => this.updateGames()))
            .subscribe(gameId => { }, error => this.handleError(error));
    }

    private updateGames() {
        this.IsLoading = true;
        this.gameClient.getFree().subscribe(games => {
            this.Games = games;
            this.IsAnyGames = this.Games && this.Games.length > 0;
        });
    }

    private handleError(error: any) {
        alert(this.errorService.Parse(error));
    }
}