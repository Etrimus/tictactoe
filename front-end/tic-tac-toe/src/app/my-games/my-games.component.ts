import { Component, ViewEncapsulation } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { ErrorService } from '../errors/error.service';
import { GameService } from '../game.service';
import { GameModel } from '../generated/dto';

@Component({
    selector: 't-my-games',
    templateUrl: './my-games.component.html',
    styleUrls: ['./my-games.component.css'],
    encapsulation: ViewEncapsulation.ShadowDom
})
export class MyGamesComponent {

    IsLoading = false;
    public IsAnyGames = false;
    public Games: GameModel[];

    constructor(
        private errorService: ErrorService,
        private gameService: GameService
    ) { }

    ngOnInit() {
        this.updateGames();
    }

    private updateGames() {
        this.IsLoading = true;
        this.gameService.GetMy().subscribe(games => {
            this.Games = games;
            this.IsAnyGames = games.length > 0;
        });
    }

    private handleError(error: any) {
        alert(this.errorService.Parse(error));
    }
}