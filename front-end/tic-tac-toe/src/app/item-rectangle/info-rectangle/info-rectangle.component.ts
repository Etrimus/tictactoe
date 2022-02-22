import { Component, Input, ViewContainerRef, ViewEncapsulation } from "@angular/core";
import { ItemRectangleComponent } from "../item-rectangle.component";

@Component({
    selector: 't-info-rectangle',
    templateUrl: './info-rectangle.component.html',
    styleUrls: ['./info-rectangle.component.css'],
    encapsulation: ViewEncapsulation.ShadowDom
})
export class InfoRectangleComponent extends ItemRectangleComponent {

    StyleSheets: Node[];
    @Input() Text: string;

    constructor(viewContainer: ViewContainerRef) {
        super(viewContainer);
    }

    public ngOnInit() {
        this.StyleSheets = Array.from(this.viewContainer.element.nativeElement.shadowRoot.querySelectorAll('style'));
    }

}