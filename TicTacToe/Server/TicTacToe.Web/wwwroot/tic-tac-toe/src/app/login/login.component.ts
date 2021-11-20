import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { GameClient } from "../generated/clients";

@Component({
    selector: 'login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.sass']
})
export class LoginCcomponent {

    constructor(private router: Router, private gameClient: GameClient) {
    }

    ngOnInit() {
        this.gameClient.getFree().subscribe(games => {
            debugger;
        });
    }

    cancel() {
        this.close();
    }

    close() {
        // Providing a `null` value to the named outlet
        // clears the contents of the named outlet
        this.router.navigate([{ outlets: { popup: null } }]);
    }

}