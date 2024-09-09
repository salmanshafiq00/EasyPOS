import { Component, Input, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR, NG_VALIDATORS, ControlValueAccessor, Validator, AbstractControl, ValidationErrors } from '@angular/forms';

@Component({
  selector: 'app-input-password',
  templateUrl: './input-password.component.html',
  styleUrls: ['./input-password.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputPasswordComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => InputPasswordComponent),
      multi: true
    }
  ]
})
export class InputPasswordComponent implements ControlValueAccessor, Validator {
  @Input() label: string = '';
  @Input() placeholder: string = '';
  @Input() required: boolean = false;
  @Input() disabled: boolean = false;
  @Input() hidden: boolean = false;
  @Input() variant: 'outlined' | 'filled' = 'outlined';
  @Input() autofocus: boolean = false;
  @Input() feedback: boolean = false;
  @Input() toggleMask: boolean = true;
  @Input() appendTo: any = null;
  @Input() inputStyleClass: string = '';
  @Input() styleClass: string = '';
  @Input() showClear: boolean = true;
  @Input() maxLength: number = null;
  @Input() promptLabel: string = null;
  @Input() mediumRegex: string = '^(((?=.*[a-z])(?=.*[A-Z]))|((?=.*[a-z])(?=.*[0-9]))|((?=.*[A-Z])(?=.*[0-9])))(?=.{6,})';
  @Input() strongRegex: string = '^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.{8,})';
  @Input() weakLabel: string = '';
  @Input() mediumLabel: string = '';
  @Input() strongLabel: string = '';
  @Input() inputId: string = '';

  value: string;
  onTouched: any = () => {};
  onChangeFn: any = (_: any) => {};

  writeValue(value: any): void {
    this.value = value;
  }

  registerOnChange(fn: any): void {
    this.onChangeFn = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  validate(control: AbstractControl): ValidationErrors | null {
    if (this.required && !control.value) {
      return { required: true };
    }
    return null;
  }

  onInputChange(event: any): void {
    this.value = event;
    this.onChangeFn(this.value);
  }

  onBlurEvent(): void {
    this.onTouched();
  }
}
