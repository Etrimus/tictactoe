import { Component, ViewEncapsulation } from "@angular/core";

@Component({
    selector: 't-item-rectangle',
    templateUrl: './item-rectangle.component.html',
    styleUrls: ['./item-rectangle.component.css'],
    encapsulation: ViewEncapsulation.ShadowDom
})
export class ItemRectangleComponent {

    IsLoading = false;

}