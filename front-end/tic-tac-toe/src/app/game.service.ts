import { Injectable } from "@angular/core";
import { from, Observable } from "rxjs";
import { tap } from "rxjs/operators";
import { CommonService } from "./common.service";
import { GameClient } from "./generated/clients";
import { GameModel } from "./generated/dto";

@Injectable({
    providedIn: 'root',
})
export class GameService {

    constructor(private gameClient: GameClient, private commonService: CommonService) { }

    private myGamesStorageKey = 'my_games';
    private myGamesStorageValueSeparator = ';'

    public Add(): Observable<string> {
        return this.gameClient.add();
    }

    public GetMy(): Observable<GameModel[]> {

        const myGames = localStorage
            .getItem(this.myGamesStorageKey)
            ?.split(this.myGamesStorageValueSeparator)
            .filter(x => {
                let isTrue = this.commonService.GuidRegExp.test(x) === true;
                return isTrue;
            });

        return !myGames || myGames.length < 1
            ? from<Array<GameModel>[]>([])
            : this.gameClient.getById(myGames).pipe(
                tap(games => {
                    localStorage.setItem(this.myGamesStorageKey, games.map(game => game.id).join(this.myGamesStorageValueSeparator));
                })
            );
    }

    public SetCrossPlayer(gameId: string, playerId: string): Observable<void> {
        return this.gameClient.setCrossPlayer(gameId, playerId).pipe(
            tap(() => this.addMyGame(gameId))
        );
    }

    public SetZeroPlayer(gameId: string, playerId: string): Observable<void> {
        return this.gameClient.setZeroPlayer(gameId, playerId).pipe(
            tap(() => this.addMyGame(gameId))
        );
    }

    private addMyGame(gameId: string): void {
        let myGamesStr = localStorage.getItem(this.myGamesStorageKey);
        if (!myGamesStr) {
            localStorage.setItem(this.myGamesStorageKey, gameId)
        } else if (!myGamesStr.includes(gameId)) {
            localStorage.setItem(this.myGamesStorageKey, `${myGamesStr}${this.myGamesStorageValueSeparator}${gameId}`);
        }
    }
}