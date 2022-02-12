import { Injectable } from "@angular/core";
import { v4 as uuidv4 } from 'uuid';
import { CommonService } from "./common.service";

@Injectable({
    providedIn: 'root',
})
export class UserService {

    constructor(private commonService: CommonService) { }

    private userIdKey = 'user-id';

    public GetUserId(): string {
        let userId = localStorage.getItem(this.userIdKey);

        if (!userId || !this.commonService.GuidRegExp.test(userId)) {
            localStorage.setItem(this.userIdKey, uuidv4())
        }

        return localStorage.getItem(this.userIdKey);
    }
}