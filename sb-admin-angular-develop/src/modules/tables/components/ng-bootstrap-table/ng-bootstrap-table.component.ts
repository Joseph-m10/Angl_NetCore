import {
    ChangeDetectionStrategy,
    ChangeDetectorRef,
    Component,
    ElementRef,
    Input,
    OnInit,
    QueryList,
    ViewChild,
    ViewChildren,
} from '@angular/core';
import { SBSortableHeaderDirective, SortEvent } from '@modules/tables/directives';
import { Country } from '@modules/tables/models';
import { CountryService } from '@modules/tables/services';
import { BehaviorSubject, Observable } from 'rxjs';
import { ChartsService } from '../../../charts/services/charts.service';

@Component({
    selector: 'sb-ng-bootstrap-table',
    changeDetection: ChangeDetectionStrategy.OnPush,
    templateUrl: './ng-bootstrap-table.component.html',
    styleUrls: ['ng-bootstrap-table.component.scss'],
})
export class NgBootstrapTableComponent implements OnInit {
    @Input() pageSize = 4;
    @Input() type: number;
    @Input() val: any;
    countries$!: Observable<Country[]>;
    tableList$ = new BehaviorSubject<any>([]);
    sortedColumn!: string;
    sortedDirection!: string;

    @ViewChildren(SBSortableHeaderDirective) headers!: QueryList<SBSortableHeaderDirective>;
    //tableList$: any;
    tableList: any;
    //tableList: any = [{'Grade':'S',Offcount:10,Oncount:20}];
    constructor(
        public countryService: CountryService,
        private changeDetectorRef: ChangeDetectorRef,
        private chartService: ChartsService
    ) {}

    async ngOnInit() {
        //this.tableList = this.val;
        //this.countryService.pageSize = this.pageSize;
        //this.countries$ = this.countryService.countries$;
        //this.total$ = this.countryService.total$;
        this.load();
    }
    load() {
        this.chartService.GetTabledata(this.type).subscribe((data: any) => {
            this.tableList$.next(data.Table)});
    }
    ngAfterViewInit() {
    }

    //onSort({ column, direction }: SortEvent) {
    //    this.sortedColumn = column;
    //    this.sortedDirection = direction;
    //    this.countryService.sortColumn = column;
    //    this.countryService.sortDirection = direction;
    //    this.changeDetectorRef.detectChanges();
    //}
}
