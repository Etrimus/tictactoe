//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.15.10.0 (NJsonSchema v10.6.10.0 (Newtonsoft.Json v9.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

/* tslint:disable */
/* eslint-disable */
// ReSharper disable InconsistentNaming



export abstract class ModelBase implements IModelBase {
    id!: string;

    constructor(data?: IModelBase) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.id = _data["Id"];
        }
    }

    static fromJS(data: any): ModelBase {
        data = typeof data === 'object' ? data : {};
        throw new Error("The abstract class 'ModelBase' cannot be instantiated.");
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["Id"] = this.id;
        return data;
    }
}

export interface IModelBase {
    id: string;
}

export class GameModel extends ModelBase implements IGameModel {
    board?: Board | undefined;
    crossPlayerId?: string | undefined;
    zeroPlayerId?: string | undefined;

    constructor(data?: IGameModel) {
        super(data);
    }

    init(_data?: any) {
        super.init(_data);
        if (_data) {
            this.board = _data["Board"] ? Board.fromJS(_data["Board"]) : <any>undefined;
            this.crossPlayerId = _data["CrossPlayerId"];
            this.zeroPlayerId = _data["ZeroPlayerId"];
        }
    }

    static fromJS(data: any): GameModel {
        data = typeof data === 'object' ? data : {};
        let result = new GameModel();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["Board"] = this.board ? this.board.toJSON() : <any>undefined;
        data["CrossPlayerId"] = this.crossPlayerId;
        data["ZeroPlayerId"] = this.zeroPlayerId;
        super.toJSON(data);
        return data;
    }
}

export interface IGameModel extends IModelBase {
    board?: Board | undefined;
    crossPlayerId?: string | undefined;
    zeroPlayerId?: string | undefined;
}

export class Board implements IBoard {
    cells?: Cell[] | undefined;
    nextTurn!: CellType;
    winner!: CellType;

    constructor(data?: IBoard) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            if (Array.isArray(_data["Cells"])) {
                this.cells = [] as any;
                for (let item of _data["Cells"])
                    this.cells!.push(Cell.fromJS(item));
            }
            this.nextTurn = _data["NextTurn"];
            this.winner = _data["Winner"];
        }
    }

    static fromJS(data: any): Board {
        data = typeof data === 'object' ? data : {};
        let result = new Board();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        if (Array.isArray(this.cells)) {
            data["Cells"] = [];
            for (let item of this.cells)
                data["Cells"].push(item.toJSON());
        }
        data["NextTurn"] = this.nextTurn;
        data["Winner"] = this.winner;
        return data;
    }
}

export interface IBoard {
    cells?: Cell[] | undefined;
    nextTurn: CellType;
    winner: CellType;
}

export class Cell implements ICell {
    number!: number;
    state!: CellType;

    constructor(data?: ICell) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.number = _data["Number"];
            this.state = _data["State"];
        }
    }

    static fromJS(data: any): Cell {
        data = typeof data === 'object' ? data : {};
        let result = new Cell();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["Number"] = this.number;
        data["State"] = this.state;
        return data;
    }
}

export interface ICell {
    number: number;
    state: CellType;
}

export enum CellType {
    None = 0,
    Zero = 1,
    Cross = 2,
}