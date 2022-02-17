import { Component, EventEmitter, Input, Output, ViewEncapsulation } from '@angular/core';
import { CellType } from 'src/app/generated/dto';
import { UserService } from 'src/app/user.service';

@Component({
    selector: 't-player-line',
    templateUrl: './player-line.component.html',
    styleUrls: ['./player-line.component.css'],
    encapsulation: ViewEncapsulation.ShadowDom
})
export class PlayerLineComponent {

    constructor(private userService: UserService) { }

    joinText = 'Присоединиться';

    private _playerId: string;

    @Input() gameId: string;
    @Input() cellType: CellType;
    playerName: string;

    get playerId(): string {
        return this._playerId;
    }

    @Input() set playerId(value: string) {
        this._playerId = value;

        const isPlayerIdIsYour = value === this.userService.GetUserId();
        this.playerName = isPlayerIdIsYour ? "Вы" : "Оппонет";
    }

    @Output() JoinButtonEvent = new EventEmitter<CellType>();

    joinButtonClick() {
        this.JoinButtonEvent.emit(this.cellType);
    }
}