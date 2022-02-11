import { Component, EventEmitter, Output, ViewContainerRef, ViewEncapsulation } from "@angular/core";

@Component({
    selector: 't-add-game',
    templateUrl: './add-game.component.html',
    styleUrls: ['./add-game.component.css'],
    encapsulation: ViewEncapsulation.ShadowDom
})
export class AddGameComponent {

    IsLoading = false;
    StyleSheets: Node[];

    @Output() AddGameEvent = new EventEmitter<any>();

    constructor(private viewContainer: ViewContainerRef) { }

    public ngOnInit() {
        this.StyleSheets = Array.from(this.viewContainer.element.nativeElement.shadowRoot.querySelectorAll('style'));
    }

    public addGame() {
        this.AddGameEvent.emit();
    }

}