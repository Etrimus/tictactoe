import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { finalize, tap } from "rxjs/operators";

@Injectable()
export class BadRequestIntercepter implements HttpInterceptor {

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        debugger;
        return next.handle(req).pipe(
            tap(event => {
                debugger;
            })
            , finalize(() => {
                debugger;
            })
        );
    }

}