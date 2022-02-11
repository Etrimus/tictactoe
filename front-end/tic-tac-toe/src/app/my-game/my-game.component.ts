import { Component, Input, ViewContainerRef, ViewEncapsulation } from "@angular/core";
import { ErrorService } from "../errors/error.service";
import { GameService } from "../game.service";
import { GameClient } from "../generated/clients";
import { GameModel } from "../generated/dto";

@Component({
    selector: 't-my-game',
    templateUrl: './my-game.component.html',
    styleUrls: ['./my-game.component.css'],
    encapsulation: ViewEncapsulation.ShadowDom
})
export class MyGameComponent {

    constructor(
        private gameClient: GameClient,
        private gameService: GameService,
        private errorService: ErrorService,
        private viewContainer: ViewContainerRef
    ) { }

    IsLoading = false;
    @Input() Game: GameModel;
    StyleSheets: Node[];

    public ngOnInit() {
        this.StyleSheets = Array.from(this.viewContainer.element.nativeElement.shadowRoot.querySelectorAll('style'));
    }

}