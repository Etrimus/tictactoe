export class ErrorService {

    public Parse(error: any): string {
        return JSON.parse(error.response).message;
    }
}