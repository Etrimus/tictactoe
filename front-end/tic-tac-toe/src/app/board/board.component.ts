import { Component, EventEmitter, Input, Output } from "@angular/core";
import { Board, Cell, CellType } from "../generated/dto";

@Component({
    selector: 't-board',
    templateUrl: './board.component.html',
    styleUrls: ['./board.component.css']
})
export class BoardComponent {
    constructor() { }

    @Input() Board: Board;
    @Input() IsInteractive = true;

    @Output() CellClickedEvent = new EventEmitter<Cell>();

    public CellClicked(cell: Cell) {
        if (this.IsInteractive && cell.state === CellType.None) {
            this.CellClickedEvent.emit(cell);
        }
    }
}