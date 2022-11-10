import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

@Injectable()
export class ChartsService {
    public _tableList$ = new BehaviorSubject<any>([]);
    
    getCharts$(): Observable<{}> {
        return of({});
    }

    API_URL = '';

    constructor(public http: HttpClient, @Inject('BASE_URL') baseUrl: string) {

        this.API_URL = baseUrl;

    }

    GetChartdata(type: number) {
        return this.http.get(this.API_URL + 'api/PMO/GetChartdata?type='+ type);
    }
    GetTabledata(type: number) {
        return this.http.get(this.API_URL + 'api/PMO/GetTabledata?type=' + type).pipe(
            tap(_ => this.test())
        );


       //return this.http.get(this.API_URL + 'api/PMO/GetTabledata?type=' + type);//.subscribe(async (data: any) => {
        //    await new Promise(f => setTimeout(f, 1000));
        //    if (data && data.Table && data.Table.length > 0) {
        //        data.Table.map((d: any) => {
        //            console.log('hjgfdsafhj', d);
        //            this._tableList$.next(d)
        //        });
        //    }
        //});
    }
    test(): void {
        console.log('test');
    }
    handleError<T>(arg0: string): (err: any, caught: Observable<Object>) => import("rxjs").ObservableInput<any> {
        throw new Error('Method not implemented.');
    }

}
