import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root',
})
export class ErrorService {

    public Parse(error: any): string {
        return error.response
            ? JSON.parse(error.response).message
            : error;
    }
}