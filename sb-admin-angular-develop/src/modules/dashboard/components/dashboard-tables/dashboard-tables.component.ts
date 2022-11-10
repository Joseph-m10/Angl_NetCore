import { ChangeDetectionStrategy, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ChartsService } from '../../../charts/services/charts.service';

@Component({
    selector: 'sb-dashboard-tables',
    changeDetection: ChangeDetectionStrategy.OnPush,
    templateUrl: './dashboard-tables.component.html',
    styleUrls: ['dashboard-tables.component.scss'],
})
export class DashboardTablesComponent implements OnInit {
    count: number;
    view: number;
    tableListt: any;
    constructor(
        private chartService: ChartsService) { }
    ngOnInit() {
        //this.chartService.GetTabledata(1).subscribe((data: any) => {
        //    if (data && data.Table && data.Table.length > 0) {
        //        this.tableListt = data.Table;
        //        console.log(data.Table);
        //    }
        //});
        //console.log('tableLis', this.tableListt);
        this.count = 2;
        this.view = 1;
    }
}
