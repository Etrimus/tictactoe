import { Component, Input, ViewContainerRef, ViewEncapsulation } from "@angular/core";
import { GameModel } from "../generated/dto";

@Component({
    selector: 't-my-game-wrap',
    templateUrl: './my-game-wrap.component.html',
    styleUrls: ['./my-game-wrap.component.css'],
    encapsulation: ViewEncapsulation.ShadowDom
})
export class MyGameWrapComponent {

    private _game: GameModel;

    constructor(
        private viewContainer: ViewContainerRef
    ) { }

    StyleSheets: Node[];
    IsGameIsFree = false;

    get Game(): GameModel {
        return this._game;
    }

    @Input() set Game(value: GameModel) {
        this.IsGameIsFree = !value.crossPlayerId || !value.zeroPlayerId;

        this._game = value;
    }

    public ngOnInit() {
        this.StyleSheets = Array.from(this.viewContainer.element.nativeElement.shadowRoot.querySelectorAll('style'));
    }

    public GameChanged(value: GameModel) {
        this.Game = value;
    }
}