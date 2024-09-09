// input-file.component.ts
import { HttpClient, HttpEvent, HttpEventType } from '@angular/common/http';
import { Component, EventEmitter, inject, Input, Output, ViewChild } from '@angular/core';
import { Observable, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ToastService } from '../../services/toast.service';
import { FileUpload } from 'primeng/fileupload';

@Component({
  selector: 'app-input-file-adv',
  templateUrl: './input-file-adv.component.html',
  styleUrls: ['./input-file-adv.component.scss']
})
export class InputFileAdvComponent {
  @Input() label: string = '';
  @Input() location: string = ''; // file-location: '/images' or '/images/student'
  @Input() required: boolean = false;
  @Input() disabled: boolean = false;
  @Input() readonly: boolean = false;
  @Input() hidden: boolean = false;
  @Input() accept: string = null;
  @Input() multiple: boolean = true;
  @Input() name: string = null;
  @Input() url: string = '/managefiles/upload';
  @Input() auto: boolean = false;
  @Input() maxFileSize: number = 2000000; // two mb
  @Input() fileLimit: number = 2;
  @Input() maxWidth: number = null;
  @Input() maxHeight: number = null;
  @Input() uploadLabel: string = 'Upload';
  @Input() chooseLabel: string = 'Browse';
  @Input() chooseIcon: string = 'pi pi-plus';
  @Input() cancelLabel: string = null;
  @Input() cancelText: boolean = false;
  @Input() cancelIcon: string = 'pi pi-times';
  @Input() uploadIcon: string = 'pi pi-upload';
  @Input() showCancelButton: boolean = true;
  @Input() showUploadButton: boolean = true;
  @Input() showProgress: boolean = true;
  @Input() mode: 'basic' | 'advanced' = 'advanced';
  @Input() customUpload: boolean = true;
  @Input() showTemplate: boolean = true;
  @Input() style: any = null;
  @Input() styleClass: string = null;
  @Input() previewWidth: number = 50;
  @Input() uploadStyleClass: string = 'p-button-successs';
  @Input() cancelStyleClass: string = null;
  @Input() removeStyleClass: string = null;
  @Input() chooseStyleClass: string = null;
  @Input() invalidFileTypeMessageSummary: string = 'Invalid File Type';
  @Input() invalidFileSizeMessageSummary: string = 'File Max Size Exceeded';
  @Input() invalidFileLimitMessageSummary: string = 'File Limit Exceed';
  @Output() uploadedFiles = new EventEmitter<any>();
  @Output() selectedFiles = new EventEmitter<any>();
  @Output() uploadError = new EventEmitter<any>();

  _fileUrls: string[] = [];

  @Input()
  get fileUrls(): string[] {
    return this._fileUrls;
  }

  set fileUrls(value: string[]) {
    this._fileUrls = value;
    this.fileUrlsChange.emit(this._fileUrls);
  }

  @Output() fileUrlsChange = new EventEmitter<string[]>();

  @ViewChild('fileUpload') fileUpload: FileUpload;

  private baseUrl = `${environment.API_BASE_URL}/api`;

  private toast = inject(ToastService)

  progressValue: number = 0;
  uploadFiles: any[] = [];

  private http = inject(HttpClient)

  onUpload(event: any) {
    this.uploadedFiles.emit(event.files);
    console.log('onupload')
  }

  onProgress(event: any) {
    this.progressValue = event.progres;
  }

  onError(event: any) {
    this.uploadError.emit(event)
  }

  onSelect(event: any) {
    this.selectedFiles.emit(event);
  }

  onUploadHandler(event: any) {

    this.upload(event.files).subscribe({
      next: (response) => {
        if (response.status === 'success') {
          this.toast.showCustom(`Upload Successfully`, 'success', '', 'file-toast', 3000);
          if(response?.body && Array.isArray(response?.body)){
            this.fileUrls = response.body?.map(x => x.filePath);
          }
          // Do something with the file path (e.g., display a message or update UI)
          this.fileUpload.clear();
        } else if (response.status === 'progress') {
          this.progressValue = response.progress; // Update progress bar
        }
      },
      error: (error) => {
        this.toast.showCustom(`Upload Failed`, 'error', '', 'file-toast', 3000);
      }
    });

  }

  private upload(files: File[]): Observable<any> {
    const formData = new FormData();
    files.forEach(file => {
      formData.append('files', file, file.name);
    });
    formData.append('location', this.location);

    return this.http.post(`${this.baseUrl}${this.url}`, formData, {
      reportProgress: true,
      withCredentials: true,
      observe: 'events'
    }).pipe(
      map((event: HttpEvent<any>) => {
        switch (event.type) {
          case HttpEventType.UploadProgress:
            return {
              status: 'progress',
              progress: Math.round(100 * event.loaded / (event.total || 1))
            };
          case HttpEventType.Response:
            return {
              status: 'success',
              body: event.body // The server should return the file path here.
            };
          default:
            return { status: 'unknown' };
        }
      })
    );
  }


}



