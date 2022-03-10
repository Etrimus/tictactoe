import { Component, Input } from "@angular/core";

@Component({
    selector: 't-item-rectangle',
    templateUrl: './item-rectangle.component.html',
    styleUrls: ['./item-rectangle.component.css']
})
export class ItemRectangleComponent {

    @Input()
    IsLoading = false;
}