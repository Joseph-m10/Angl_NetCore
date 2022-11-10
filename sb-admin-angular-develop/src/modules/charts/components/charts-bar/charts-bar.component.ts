import { HttpClient } from '@angular/common/http';
import { ChartsService } from '../../services/charts.service';
import {
    AfterViewInit,
    ChangeDetectionStrategy,
    Component,
    ElementRef,
    Inject,
    Input,
    OnInit,
    ViewChild,
} from '@angular/core';
import Chart from 'chart.js';

@Component({
    selector: 'sb-charts-bar',
    changeDetection: ChangeDetectionStrategy.OnPush,
    templateUrl: './charts-bar.component.html',
    styleUrls: ['charts-bar.component.scss'],
})
export class ChartsBarComponent implements OnInit, AfterViewInit {
    @ViewChild('myBarChart') myBarChart!: ElementRef<HTMLCanvasElement>;
    @Input() type: number;
    chart!: Chart;
    API_URL = '';
    labelArray: Array<string>=[];
    valArray: any = [];
    valArray1: any = [];
    constructor(public http: HttpClient, private chartService: ChartsService, @Inject('BASE_URL') baseUrl: string) {
        this.API_URL = baseUrl;
    }
    ngOnInit() {
    }

    ngAfterViewInit() {
        this.chartService.GetChartdata(this.type).subscribe((data: any) => {
            if (data && data.Table && data.Table.length > 0 && data.Table1 && data.Table1.length > 0) {
                data.Table.map((d: any) => {
                    this.labelArray.push(d.Grade);
                    this.valArray.push(d.count);
                })
                data.Table1.map((d: any) => {
                    this.valArray1.push(d.count);
                })
                this.chart = new Chart(this.myBarChart.nativeElement, {
                    type: 'bar',
                    data: {
                        labels: this.labelArray,
                        //labels: ['January', 'February', 'March', 'April', 'May', 'June'],
                        datasets: [
                            {
                                label: 'Count',
                                backgroundColor: 'rgba(2,117,216,1)',
                                borderColor: 'rgba(2,117,216,1)',
                                data: this.valArray,
                                //data: [4215, 5312, 6251, 7841, 9821, 14984],
                            },
                            {
                                label: 'Count',
                                backgroundColor: 'rgb(0, 191, 255)',
                                borderColor: 'rgb(0, 191, 255)',
                                data: this.valArray1,
                                //data: [4215, 5312, 6251, 7841, 9821, 14984],
                            },
                        ],
                    },
                    options: {
                        scales: {
                            xAxes: [
                                {
                                    time: {
                                        unit: 'month',
                                    },
                                    gridLines: {
                                        display: false,
                                    },
                                    ticks: {
                                        maxTicksLimit: 15,
                                    },
                                },
                            ],
                            yAxes: [
                                {
                                    ticks: {
                                        min: 0,
                                        max: 1000,
                                        maxTicksLimit: 15,
                                    },
                                    gridLines: {
                                        display: true,
                                    },
                                },
                            ],
                        },
                        legend: {
                            display: false,
                        },
                    },
                });
            }
        });
    }
}
