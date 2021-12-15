import { Component, EventEmitter, Input, Output, ViewEncapsulation } from '@angular/core';
import { CellType } from 'src/app/generated/dto';

@Component({
    selector: 't-player-line',
    templateUrl: './player-line.component.html',
    styleUrls: ['./player-line.component.css'],
    encapsulation: ViewEncapsulation.ShadowDom
})
export class PlayerLineComponent {

    joinText = 'Присоединиться';

    @Input() gameId: string;
    @Input() cellType: CellType;
    @Input() playerId: string;
    @Input() playerName: string;

    @Output() JoinButtonEvent = new EventEmitter<CellType>();

    joinButtonClick() {
        this.JoinButtonEvent.emit(this.cellType);
    }
}