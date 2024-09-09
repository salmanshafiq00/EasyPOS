// input-file.component.ts
import { Component, Input } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR, NG_VALIDATORS, AbstractControl, ValidationErrors, Validator } from '@angular/forms';
import { ToastService } from '../../services/toast.service';
import { FileUpload } from 'primeng/fileupload';

@Component({
  selector: 'app-input-file',
  templateUrl: './input-file.component.html',
  styleUrls: ['./input-file.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: InputFileComponent,
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: InputFileComponent,
      multi: true
    }
  ]
})
export class InputFileComponent implements ControlValueAccessor, Validator {
  @Input() label: string = '';
  @Input() required: boolean = false;
  @Input() disabled: boolean = false;
  @Input() readonly: boolean = false;
  @Input() hidden: boolean = false;
  @Input() autofocus: boolean = false;
  @Input() inputId: string = null;
  @Input() accept: string = null;
  @Input() multiple: boolean = true;
  @Input() name: string = null;
  @Input() maxFileSize: number = 2000000; // two mb
  @Input() fileLimit: number = 2;
  @Input() maxWidth: number = null;
  @Input() maxHeight: number = null;
  @Input() chooseLabel: string = 'Browse';
  @Input() chooseIcon: string = 'pi pi-upload';
  @Input() cancelLabel: string = null;
  @Input() cancelText: boolean = false;
  @Input() cancelIcon: string = 'pi pi-times';
  @Input() showCancelButton: boolean = true;
  @Input() mode: 'basic' | 'advanced' = 'basic';
  @Input() customUpload: boolean = true;
  @Input() showTemplate: boolean = true;
  @Input() invalidFileTypeMessageSummary: string = 'Invalid File Type';
  @Input() invalidFileSizeMessageSummary: string = 'File Max Size Exceeded';
  @Input() invalidFileLimitMessageSummary: string = 'File Limit Exceed';

  // fileUpload: FileUpload = null;

  value: File[] = [];
  onTouched: () => void = () => { };
  onChangeFn: (value: File[]) => void = () => { };

  constructor(private toastService: ToastService) { }

  writeValue(value: File[]): void {
    this.value = value || [];
  }

  registerOnChange(fn: (value: File[]) => void): void {
    this.onChangeFn = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  validate(control: AbstractControl): ValidationErrors | null {
    if (this.required && this.value?.length === 0) {
      return { required: true };
    }
    return null;
  }

  onClearFiles(){
    this.value = [];
    this.updateValue();
    console.log(this.value)
  }

  onFileUpload(event: any, fileInput: any): void {

    let hasValidFiles: boolean = true;

    const p_fileuploadValMsgs = fileInput.msgs;

    const files = event.currentFiles;
    const validationPromises: Promise<void>[] = [];

    if(p_fileuploadValMsgs?.length > 0){

      const firstValidationMsg = p_fileuploadValMsgs[0];

      if (firstValidationMsg?.summary === 'Invalid File Type') {
        this.toastService.showWarn(`Allowed file type ${this.accept}.`, 'Invalid File Type');
      }else if (firstValidationMsg?.summary === 'File Max Size Exceeded') {
        this.toastService.showWarn(`Allowed file size ${this.maxFileSize}.`, 'Invalid File Size');
      }else if (firstValidationMsg?.summary === 'File Limit Exceed') {
        this.toastService.showWarn(`Only ${this.fileLimit} file allowed.`, 'File Limit Exceed');
      }
      hasValidFiles = false;
  
    }

    if (files.length > 0) {
      if (!this.isValidFileCount(files)) {
        this.toastService.showWarn(`Only ${this.fileLimit} file allowed.`, 'File Limit Exceeded');
        hasValidFiles = false;
      } else if (this.maxWidth || this.maxHeight) {
        for (const file of files) {
          validationPromises.push(this.validateImageDimensions(file));
        }
      }
    }

    if (files.length > 0) {
      if (this.maxWidth || this.maxHeight) {
        for (const file of files) {
          validationPromises.push(this.validateImageDimensions(file));
        }
      }
    }

    if (hasValidFiles) {
      Promise.all(validationPromises).then(() => {
        this.value = [];
        this.value = files;
        this.updateValue();
        fileInput.clear();
        console.log(this.value)
      }).catch(() => {
        this.value = [];
        this.updateValue();
        fileInput.clear();
        console.log(this.value)
      });
    } else {
      this.value = [];
      this.updateValue();
      fileInput.clear();
      console.log(this.value)
    }
  }



  private isValidFileCount(files: File[]): boolean {
    return files.length <= this.fileLimit;
  }

  private hasMaxSizeExceed(files: File[]): boolean {
    return files.some(file => file.size > this.maxFileSize);
  }

  private hasInvalidType(files: File[]): boolean {
    if (!this.accept) return false;

    const acceptPatterns = this.accept.split(',').map(pattern => pattern.trim());

    return files.some(file => !this.isFileTypeAccepted(file, acceptPatterns));
  }

  private isFileTypeAccepted(file: File, acceptPatterns: string[]): boolean {
    return acceptPatterns.some(pattern => {
      if (pattern.startsWith('.')) {
        // Match by file extension
        return file.name.endsWith(pattern);
      } else if (pattern.endsWith('/*')) {
        // Match by file type (e.g., image/*)
        const baseType = pattern.slice(0, -1); // 'image/*' -> 'image/'
        return file.type.startsWith(baseType);
      } else {
        // Exact match (e.g., image/jpeg)
        return file.type === pattern;
      }
    });
  }

  private validateImageDimensions(file: File): Promise<void> {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.onload = (e: any) => {
        const img = new Image();
        img.onload = () => {
          const width = img.width;
          const height = img.height;
          if ((this.maxWidth && width > this.maxWidth) && (this.maxHeight && height > this.maxHeight)) {
            this.toastService.showWarn(`Image dimensions should be within ${this.maxWidth}x${this.maxHeight}px.`, 'Invalid Image Resolution');
            reject();
          } else if ((this.maxWidth && width > this.maxWidth)) {
            this.toastService.showWarn(`Max width ${this.maxWidth}px is allowed.`, 'Invalid Image Resolution');
            reject();
          } else if ((this.maxHeight && height > this.maxHeight)) {
            this.toastService.showWarn(`Max height ${this.maxWidth}px is allowed.`, 'Invalid Image Resolution');
            reject();
          } else {
            resolve();
          }
        };
        img.onerror = () => reject();
        img.src = e.target.result;
      };
      reader.onerror = () => reject();
      reader.readAsDataURL(file);
    });
  }

  private updateValue(): void {
    this.onTouched();
    this.onChangeFn(this.value);
  }

}


