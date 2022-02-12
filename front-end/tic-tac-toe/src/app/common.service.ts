import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root',
})
export class CommonService {
    private guidLetterExp = '[0-9A-Fa-f]';

    public GuidRegExp = new RegExp(`^(${this.guidLetterExp}{8}[-]${this.guidLetterExp}{4}[-]${this.guidLetterExp}{4}[-]${this.guidLetterExp}{4}[-]${this.guidLetterExp}{12})$`);
}
