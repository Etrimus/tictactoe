import { Component, ViewContainerRef, ViewEncapsulation } from "@angular/core";
import { ErrorService } from "src/app/errors/error.service";
import { GameClient } from "src/app/generated/clients";

@Component({
    selector: 't-add-game',
    templateUrl: './add-game.component.html',
    styleUrls: ['./add-game.component.css'],
    encapsulation: ViewEncapsulation.ShadowDom
})
export class AddGameComponent {

    IsLoading = false;
    StyleSheets: Node[];

    constructor(
        private gameClient: GameClient,
        private errorService: ErrorService,
        private viewContainer: ViewContainerRef
    ) { }

    public ngOnInit() {
        this.StyleSheets = Array.from(this.viewContainer.element.nativeElement.shadowRoot.querySelectorAll('style'));
    }
}