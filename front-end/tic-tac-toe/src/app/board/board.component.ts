import { Component, EventEmitter, Input, Output, ViewEncapsulation } from "@angular/core";
import { Board, Cell } from "../generated/dto";

@Component({
    selector: 't-board',
    templateUrl: './board.component.html',
    styleUrls: ['./board.component.css'],
    encapsulation: ViewEncapsulation.ShadowDom
})
export class BoardComponent {
    constructor() { }

    @Input() Board: Board;
    @Input() IsInteractive = true;

    @Output() CellClickedEvent = new EventEmitter<Cell>();

    public CellClicked(cell: Cell) {
        if (this.IsInteractive) {
            this.CellClickedEvent.emit(cell);
        }
    }
}