import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';

const EXCEL_TYPE = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8';
const EXCEL_EXTENSION = '.xlsx';

@Injectable({
    providedIn: 'root'
})


export class UploadService {

    API_URL = '';

    constructor(public http: HttpClient, @Inject('BASE_URL') baseUrl: string) {

        this.API_URL = baseUrl;

    }

    GetTemplateData(tempId: number) {
        return this.http.get(this.API_URL + 'api/PMO/GetTemplateData?tempId=' + tempId);
    }


    exportWidgetData(DataList: any, fileName: string, sheetName: string) {

        /*this XLSX.utils.json_to_sheet convert json or array into worksheet*/
        const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(DataList);


        const wb: XLSX.WorkBook = { SheetNames: [], Sheets: {} };
        //  /*declare the worksheet data and sheet name for a workbook*/
        //const workbook: XLSX.WorkBook = { Sheets: { 'data': worksheet }, SheetNames: ['data'] };
        wb.SheetNames.push(sheetName);
        wb.Sheets[sheetName] = worksheet;

        //  /*attempts to write the   workbook wb*/
        const excelBuffer: any = XLSX.write(wb, { bookType: 'xlsx', type: 'array' });
        //this.saveAsExcelFile(excelBuffer, fileName);
        const data: Blob = new Blob([excelBuffer], {
            type: EXCEL_TYPE
        });
        FileSaver.saveAs(data, fileName + EXCEL_EXTENSION);
    }

}
