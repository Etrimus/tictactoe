import { Component, Input, ViewContainerRef, ViewEncapsulation } from "@angular/core";

@Component({
    selector: 't-item-rectangle',
    templateUrl: './item-rectangle.component.html',
    styleUrls: ['./item-rectangle.component.css'],
    encapsulation: ViewEncapsulation.ShadowDom
})
export class ItemRectangleComponent {

    @Input()
    IsLoading = false;

    @Input()
    set StyleNodes(value: Node[]) {
        value.forEach(node => this.viewContainer.element.nativeElement.shadowRoot.appendChild(node));
    }

    constructor(private viewContainer: ViewContainerRef) { }

}