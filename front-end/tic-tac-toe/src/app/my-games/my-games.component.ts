import { Component, ViewEncapsulation } from '@angular/core';
import { ErrorService } from '../errors/error.service';
import { GameClient } from '../generated/clients';
import { GameModel } from '../generated/dto';
import { UserService } from '../user.service';

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
        private userService: UserService,
        private gameClient: GameClient
    ) { }

    public ngOnInit() {
        this.updateGames();
    }

    private updateGames() {
        this.IsLoading = true;
        this.gameClient.getByPlayerId(this.userService.GetUserId()).subscribe(games => {
            this.Games = games;
            this.IsAnyGames = games.length > 0;
        });
    }

    private handleError(error: any) {
        alert(this.errorService.Parse(error));
    }
}