import { Component, Input } from "@angular/core";
import { ItemRectangleComponent } from "../item-rectangle.component";

@Component({
    selector: 't-info-rectangle',
    templateUrl: './info-rectangle.component.html',
    styleUrls: ['./info-rectangle.component.css']
})
export class InfoRectangleComponent extends ItemRectangleComponent {

    StyleSheets: Node[];
    @Input() Text: string;

}