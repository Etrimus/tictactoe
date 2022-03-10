import { Component, EventEmitter, Output } from "@angular/core";

@Component({
    selector: 't-add-game',
    templateUrl: './add-game.component.html',
    styleUrls: ['./add-game.component.css']
})
export class AddGameComponent {

    IsLoading = false;
    StyleSheets: Node[];

    @Output() AddGameEvent = new EventEmitter<any>();

    public addGame() {
        this.AddGameEvent.emit();
    }

}