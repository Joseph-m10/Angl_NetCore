import { formatDate } from '@angular/common';
import { HttpClient, HttpRequest } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, ElementRef, OnInit, Renderer2, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';
import { take } from 'rxjs/operators';
import { UploadService } from '../../services/UploadService';

@Component({
    selector: 'sb-upload-file',
    changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './upload-file.component.html',
  styleUrls: ['./upload-file.component.scss']
})
export class UploadFileComponent implements OnInit {
    @ViewChild('progressbar', { static: false }) progressbar: ElementRef;
    @ViewChild('progress_status', { static: false }) progress_status: ElementRef;
    @ViewChild('chooseFileBtn', { static: false }) chooseFileBtn: ElementRef;

    result$ = new BehaviorSubject<string>('');
    resultval = this.result$.asObservable();
    excelResult: any = [];
    selectedvalue: any = 0;
    enabledownload: boolean = false;
    fileSize = '0.0KB';
    fileType: any;
    status: string;
    uploadForm: FormGroup;
    fileNameHolder: string ='Choose The File To Upload';
    showstatus: boolean = true;
    currentDate: string;
    constructor(public uploadService: UploadService, private renderer: Renderer2, private formBuilder: FormBuilder, public http: HttpClient) { }
    dateUpload: any;

    ngOnInit(): void {

        let tday = new Date();
        let dd = tday.getDate().toString();
        var mm = (tday.getMonth() + 1).toString(); //January is 0!
        var yyyy = tday.getFullYear().toString();
        if (parseInt(dd) < 10) {
            dd = '0' + dd
        }
        if (parseInt(mm) < 10) {
            mm = '0' + mm
        }

        tday = new Date(yyyy + '-' + mm + '-' + dd);

        const datefield = document.getElementById('datefield') as HTMLElement;
        datefield.setAttribute("max", yyyy + '-' + mm + '-' + dd);

        this.uploadForm = this.formBuilder.group({
            input: ['']
        });
        this.dateUpload = formatDate(new Date(), 'yyyy-MM-dd', 'en-US');
        this.currentDate = formatDate(new Date(), 'dd-MM-yyyy', 'en-US');
        const up = document.getElementById('downlloadBTN') as HTMLElement;
        up.style.pointerEvents = "none";
        up.style.cursor = "default";
        up.style.opacity = "0.5";
        const downlloadBTN = document.getElementById('downlloadBTN') as HTMLElement;

        downlloadBTN.style.pointerEvents = "none";
        downlloadBTN.style.cursor = "default";
        downlloadBTN.style.opacity = "0.5";
  }

    openFile(e: any) {
        e.preventDefault();
        this.chooseFileBtn.nativeElement.click();
    }

    btnnclick() {
        //alert('ok');
        this.uploadService.GetTemplateData(1).subscribe((data: any) => {
            this.excelResult = JSON.parse(data);
            this.uploadService.exportWidgetData(this.excelResult.Table, "FC Report Template", "Sheet1");
        });
    }

    selectchange(e: any) {
        this.selectedvalue = e.target.value;
        const downlloadBTN = document.getElementById('downlloadBTN') as HTMLElement;
        const up = document.getElementById('up') as HTMLElement;
        if (this.selectedvalue > 0) {
            downlloadBTN.style.pointerEvents = "auto";
            downlloadBTN.style.cursor = "pointer";
            downlloadBTN.style.opacity = "1";

            up.style.pointerEvents = "auto";
            up.style.cursor = "pointer";
            up.style.opacity = "1";
        }
        else {
            downlloadBTN.style.pointerEvents = "none";
            downlloadBTN.style.cursor = "default";
            downlloadBTN.style.opacity = "0.5";

            up.style.pointerEvents = "none";
            up.style.cursor = "default";
            up.style.opacity = "0.5";
        }
    }

    fileChange(e: any, files: any) {

        var type = e.target.files[0].name;
        var size = e.target.files[0].size;
        var fSExt = new Array('Bytes', 'KB', 'MB', 'GB'),
            i = 0; while (size > 900) { size /= 1024; i++; }
        this.fileSize = (Math.round(size * 100) / 100) + ' ' + fSExt[i];
        let x = type.split(".");
        this.fileType = x[1];
        if (this.fileType != "xlsx") {
            this.renderer.addClass(this.progress_status.nativeElement, 'error-status');
            this.status = 'Please upload correct data feed template'
            setTimeout(() => {
                this.renderer.removeClass(this.progress_status.nativeElement, 'error-status');
                this.status = ''
            }, 3000);

            return
        }

        if (e.target.files.length > 0) {
            const file = e.target.files[0];
            this.uploadForm.get('input')?.setValue(file);
        }

        let initialValue = 0;
        this.renderer.setAttribute(this.progressbar.nativeElement, 'aria-valuenow', initialValue.toString());
        this.renderer.setStyle(this.progressbar.nativeElement, 'width', initialValue + '%');
        let progress = parseInt(this.progressbar.nativeElement.getAttribute('aria-valuenow'));
        this.result$.next('Uploading...');
        this.renderer.addClass(this.progress_status.nativeElement, 'uploading-status');
        this.renderer.removeClass(this.progress_status.nativeElement, 'completed-status');

        const formData1 = new FormData();
        formData1.append('file', this.uploadForm.get('input')?.value);
        formData1.append('date', '2022/03/31');
        const uploadReq = new HttpRequest('POST', `api/PMO/UploadDataFeedTemplate`, formData1, {});
        this.http.request(uploadReq).subscribe((event: any) => {
            if (event && event.body) {
                if (event.body === 'Success') {
                    this.result$.next('Completed');
                    this.renderer.removeClass(this.progress_status.nativeElement, 'error-status');
                    this.renderer.addClass(this.progress_status.nativeElement, 'completed-status');
                    this.renderer.removeClass(this.progress_status.nativeElement, 'uploading-status');
                    this.status = 'Completed';

                    let tempFileName = e.srcElement.value.split('\\');
                    this.fileNameHolder = tempFileName[tempFileName.length - 1];
                    //alert('lkjfs');
                    while (progress < 100) {
                        progress = progress + 10;
                        this.renderer.setAttribute(this.progressbar.nativeElement, 'aria-valuenow', progress.toString());
                        this.renderer.setStyle(this.progressbar.nativeElement, 'width', progress + '%');
                        //setTimeout(() => {
                        //    if (progress < 100) {
                        //        this.status = 'Uploading1...';
                        //        this.renderer.removeClass(this.progress_status.nativeElement, 'error-status');
                        //        this.renderer.addClass(this.progress_status.nativeElement, 'uploading-status');
                        //        this.renderer.removeClass(this.progress_status.nativeElement, 'completed-status');
                        //    }
                        //    else {
                        //        this.renderer.removeClass(this.progress_status.nativeElement, 'error-status');
                        //        if (updloadStatus) {
                        //            this.status = 'Completed';
                        //            console.log(this.status);
                        //            this.renderer.addClass(this.progress_status.nativeElement, 'completed-status');
                        //            this.renderer.removeClass(this.progress_status.nativeElement, 'uploading-status');
                        //        }
                        //        else {
                        //            this.status = 'Uploading2...';
                        //            this.renderer.addClass(this.progress_status.nativeElement, 'uploading-status');
                        //            this.renderer.removeClass(this.progress_status.nativeElement, 'completed-status');
                        //        }
                        //    }
                        //}, 1000)
                    }
                }
                else if (event.body === 'Failure') {
                    this.result$.next('Upload failed');
                    this.renderer.addClass(this.progress_status.nativeElement, 'error-status');
                    this.renderer.removeClass(this.progress_status.nativeElement, 'completed-status');
                    this.renderer.removeClass(this.progress_status.nativeElement, 'uploading-status');
                    this.status = 'Upload failed';
                }
            }
        });
        console.log('sdfasdfasdfasdfasd', this.result$);
        

        //const formData = new FormData();
        //formData.append('file', this.uploadForm.get('input').value);

        //this.http.post<any>(this.SERVER_URL + 'api/Upload/uploadDataFeedTemplate', formData).subscribe(
        //  (res) => console.log(res),
        //  (err) => console.log(err)
        //);

        //switch (this.value) {
        //    case 1 || 2: {
        //        this.getDashboard(this.data.ConnectionName);
        //        const formData1 = new FormData();
        //        formData1.append('file', this.uploadForm.get('input')?.value);
        //        formData1.append('date', '2022/03/31');
        //        this.apiService.authenticationMethod().subscribe((data: any) => {
        //            if (data == 1) {
        //                const uploadReq = new HttpRequest('POST', `api/Upload/UploadDataFeedTemplate?Flag=` + this.value, formData1, {});
        //                this.http.request(uploadReq).subscribe(event => {
        //                });
        //            }
        //            else {
        //                token = localStorage.getItem('currentUser');
        //                const uploadReq = new HttpRequest('POST', `api/Upload/UploadDataFeedTemplate?Flag=` + this.value, formData1, {
        //                    headers: new HttpHeaders({
        //                        "enctype": "multipart/form-data",
        //                        "Authorization": "Bearer " + token
        //                    })
        //                });
        //                this.http.request(uploadReq).subscribe(event => {
        //                });
        //            }
        //        });
        //    }
        //        break;
        //    case 3: {
        //        this.getDashboard(this.selectedDBlist);
        //        if (this.data.UploadType == 8) {

        //            this.Uploadparameter(files, this.selectedDBlist, 'M');
        //        }
        //        else {
        //            const formData = new FormData();

        //            for (let file of files) {
        //                formData.append(file.name, file, this.selectedDBlist);
        //            }
        //            this.apiService.authenticationMethod().subscribe((data: any) => {
        //                if (data == 1) {
        //                    this.http.post(this.API_URL + `api/Upload/UploadExcelData?configureFlag=` + this.data.UploadType, formData, {}).subscribe((data: any) => {
        //                        this.status = data == "False" || data == "failure" ? '' : this.status;
        //                        this.showstatus = data == "False" || data == "failure" ? false : true;
        //                    });
        //                }
        //                else {
        //                    token = localStorage.getItem('currentUser');
        //                    this.http.post(this.API_URL + `api/Upload/UploadExcelData?configureFlag=` + this.data.UploadType, formData, {
        //                        headers: new HttpHeaders({
        //                            "enctype": "multipart/form-data",
        //                            "Authorization": "Bearer " + token
        //                        })
        //                    }).subscribe((data: any) => {
        //                        this.status = data == "False" || data == "failure" ? '' : this.status;
        //                        this.showstatus = data == "False" || data == "failure" ? false : true;
        //                    });
        //                }
        //            });
        //            //const uploadReq = new HttpRequest('POST', `api/Upload/UploadExcelData?configureFlag=` + this.data.UploadType, formData, {
        //            //});

        //            //this.http.request(uploadReq).subscribe(event => {
        //            //});


        //            files = null;
        //        }
        //    }
        //        break;
        //    case 4: {

        //        this.logo.emit(files);


        //    }
        //        break;
        //    case 5: {
        //        const target: HTMLInputElement = e.target;
        //        this.files = e.target.files;
        //        if (target.files.length === 0) {
        //            throw new Error('Error');
        //        }
        //        if (target.files.length > 1) {
        //            throw new Error('Cannot use multiple files');
        //        }
        //        const reader: FileReader = new FileReader();
        //        this.readerExcel(reader);
        //        reader.readAsArrayBuffer(target.files[0]);
        //        this.sheetBufferRender = target.files[0];
        //    }
        //        break;
        //    case 6: {
        //        this.getDashboard(this.data.ConnectionName);
        //        const formData1 = new FormData();
        //        formData1.append('file', this.uploadForm.get('input').value);
        //        formData1.append('frequency', this.frequency);
        //        formData1.append('date', this.date);
        //        formData1.append('templateId', this.templateId);
        //        this.apiService.rawDumpUpload(formData1).subscribe((data: any) => {
        //            this.status = data == "False" || data == "failure" ? '' : 'Completed';
        //            this.showstatus = data == "False" || data == "failure" ? false : true;
        //            updloadStatus = true;
        //            this.renderer.addClass(this.progress_status.nativeElement, 'completed-status');
        //            this.renderer.removeClass(this.progress_status.nativeElement, 'uploading-status');

        //        });

        //    }
        //        break;
        //}
        

        
    }

    cancelUpload(e: any) {
        this.result$.next('');
        e.stopPropagation();
        this.fileNameHolder = 'Choose The File To Upload';
        this.fileSize = '0.0 KB'
        this.renderer.setAttribute(this.progressbar.nativeElement, 'aria-valuenow', '0');
        this.renderer.setStyle(this.progressbar.nativeElement, 'width', '0');
        this.status = '';
        this.showstatus = true;
        this.renderer.removeClass(this.progress_status.nativeElement, 'completed-status');
        this.renderer.removeClass(this.progress_status.nativeElement, 'uploading-status');
        this.chooseFileBtn.nativeElement.value = '';
    }
}
