import { Component, EventEmitter, Input, Output } from "@angular/core";
import { Cell, CellType } from "../generated/dto";
import { CellCaptionPipe } from "./cell-caption.pipe";

@Component({
    selector: 't-cell',
    templateUrl: './cell.component.html',
    styleUrls: ['./cell.component.css']
})
export class CellComponent {

    private _cell: Cell;

    constructor(private cellCaptionPipe: CellCaptionPipe) { }

    IsMouseOver = false;
    Text: string;

    @Input() IsEnabled = true;

    @Input() get Cell() {
        return this._cell;
    }

    set Cell(value: Cell) {
        this._cell = value;
        this.Text = this.cellCaptionPipe.transform(this._cell);
    }

    @Input() NextTurn: CellType

    @Output() ClickedEvent = new EventEmitter<Cell>();

    Clicked() {
        this.ClickedEvent.emit(this.Cell);
    }

    MouseOver() {
        if (!this.IsEnabled)
            return;

        if (this.Cell.state === CellType.None) {
            this.IsMouseOver = true;
            this.Text = this.cellCaptionPipe.transform(this.NextTurn);
        }
    }

    MouseOut() {
        if (!this.IsEnabled)
            return;

        this.IsMouseOver = false;
        this.Text = this.cellCaptionPipe.transform(this.Cell);
    }
}