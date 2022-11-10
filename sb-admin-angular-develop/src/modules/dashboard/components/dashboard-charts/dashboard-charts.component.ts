import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';

@Component({
    selector: 'sb-dashboard-charts',
    changeDetection: ChangeDetectionStrategy.OnPush,
    templateUrl: './dashboard-charts.component.html',
    styleUrls: ['dashboard-charts.component.scss'],
})
export class DashboardChartsComponent implements OnInit {
    count: number;
    view: number;
    constructor() { }
    ngOnInit() {
        this.count = 2;
        this.view = 1;
    }
}
