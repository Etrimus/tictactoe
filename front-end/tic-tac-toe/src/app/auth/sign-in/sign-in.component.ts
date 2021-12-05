import { Component } from "@angular/core";
import { Router } from "@angular/router";

@Component({
    selector: 'sign-in',
    templateUrl: './sign-in.component.html',
    styleUrls: ['./sign-in.component.sass']
})
export class SignInComponent {

    constructor(private router: Router) { }

    cancel(): void {
        this.close();
    }

    close() {
        this.router.navigate([{ outlets: { popup: null } }]);
    }

}