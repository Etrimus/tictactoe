import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root',
})
export class ErrorService {

    public Parse(error: any): string {
        return JSON.parse(error.response).message;
    }
}