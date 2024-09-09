import { Component, Input } from '@angular/core';
import { AbstractControl } from '@angular/forms';

@Component({
  selector: 'app-validator-msg',
  templateUrl: './validator-msg.component.html',
  styleUrls: ['./validator-msg.component.scss']
})
export class ValidatorMsgComponent {
  @Input() control: AbstractControl | null = null;
  @Input() validationMessages: { [key: string]: string | ((params: any) => string) } = {};
  @Input() styleClass: string = '';

  defaultMessages = {
    required: 'This field is required',
    pattern: 'Please match the requested format',
    minLength: (params: any) => `The minimum length is ${params.requiredLength} characters`,
    maxLength: (params: any) => `The maximum length is ${params.requiredLength} characters`
  };

  getErrorMessage(errorKey: string, errorParams: any): string {
    const customMessage = this.validationMessages[errorKey];
    if (customMessage) {
      return typeof customMessage === 'function' ? customMessage(errorParams) : customMessage;
    }

    const defaultMessage = this.defaultMessages[errorKey];
    return typeof defaultMessage === 'function' ? defaultMessage(errorParams) : defaultMessage;
  }

  hasError(errorKey: string): boolean {
    return this.control?.hasError(errorKey) ?? false;
  }

  isTouched(): boolean {
    return this.control?.touched ?? false;
  }

  get errorKeys(): string[] {
    return this.control ? Object.keys(this.control.errors || {}) : [];
  }
}
