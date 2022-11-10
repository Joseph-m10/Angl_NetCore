import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';

@Injectable()
export class DashboardService {

    getDashboard$(): Observable<{}> {
        return of({});
    }


    API_URL = '';

    constructor(public http: HttpClient, @Inject('BASE_URL') baseUrl: string) {

        this.API_URL = baseUrl;

    }
    GetTabledata(type: number) {
        return this.http.get(this.API_URL + 'api/PMO/GetTabledata?type=' + type);//.pipe(
        //    tap(_ => this.test())
        //);
    }

}
