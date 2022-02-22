import { Pipe, PipeTransform } from "@angular/core";
import { Cell, CellType } from "../generated/dto";

@Pipe({ name: 'cellCaption' })
export class CellCaptionPipe implements PipeTransform {

    constructor() { }

    transform(value: CellType | Cell, ..._args: any[]) {

        let cellType = (<Cell>value).state;

        if (!cellType) {
            cellType = <CellType>value;
        }

        switch (cellType) {
            case CellType.Cross:
                return 'Х'
            case CellType.Zero:
                return 'O'
        }

        return '';
    }
}