import { Injectable } from "@angular/core";
import { from, Observable } from "rxjs";
import { tap } from "rxjs/operators";
import { CommonService } from "./common.service";
import { GameState } from "./game-state";
import { GameClient } from "./generated/clients";
import { CellType, GameModel } from "./generated/dto";
import { UserService } from "./user.service";

@Injectable({
    providedIn: 'root',
})
export class GameService {

    constructor(private gameClient: GameClient, private commonService: CommonService, private userService: UserService) { }

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
                tap({ next: games => localStorage.setItem(this.myGamesStorageKey, games.map(game => game.id).join(this.myGamesStorageValueSeparator)) })
            );
    }

    public SetCrossPlayer(gameId: string, playerId: string): Observable<void> {
        return this.gameClient.setCrossPlayer(gameId, playerId).pipe(
            tap({ complete: () => this.addMyGame(gameId) })
        );
    }

    public SetZeroPlayer(gameId: string, playerId: string): Observable<void> {
        return this.gameClient.setZeroPlayer(gameId, playerId).pipe(
            tap({ complete: () => this.addMyGame(gameId) })
        );
    }

    public GetState(game: GameModel): GameState {
        const playerCellTypes: CellType[] = [];

        if (game.crossPlayerId == this.userService.GetUserId()) {
            playerCellTypes.push(CellType.Cross)
        }
        if (game.zeroPlayerId == this.userService.GetUserId()) {
            playerCellTypes.push(CellType.Zero)
        }

        if (game.board.winner !== CellType.None) {
            return playerCellTypes.includes(game.board.winner) ? GameState.YouWin : GameState.YouLose;
        } else {
            if (game.board.nextTurn === CellType.None) {
                return GameState.Draw;
            } else {
                return playerCellTypes.includes(game.board.nextTurn) ? GameState.YourTurn : GameState.OpponentTurn;
            }
        }
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