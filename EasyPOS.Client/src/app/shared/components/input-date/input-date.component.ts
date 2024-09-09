import { Component, Input, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR, NG_VALIDATORS, ControlValueAccessor, Validator, AbstractControl, ValidationErrors } from '@angular/forms';

@Component({
  selector: 'app-input-date',
  templateUrl: './input-date.component.html',
  styleUrls: ['./input-date.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputDateComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => InputDateComponent),
      multi: true
    }
  ]
})
export class InputDateComponent implements ControlValueAccessor, Validator {
  @Input() label: string = '';
  @Input() placeholder: string = 'dd/mm/yyyy';
  @Input() required: boolean = false;
  @Input() disabled: boolean = false;
  @Input() readonly: boolean = false;
  @Input() hidden: boolean = false;
  @Input() variant: 'outlined' | 'filled' = 'outlined';
  @Input() autofocus: boolean = false;
  @Input() dateFormat: string = 'dd/mm/yy';
  @Input() showIcon: boolean = true;
  @Input() showOnFocus: boolean = true;
  @Input() iconDisplay: 'input' | 'button' = 'input';
  @Input() readonlyInput: boolean = false;
  @Input() selectionMode: 'single' | 'multiple' | 'range' = 'single';
  @Input() minDate?: Date;
  @Input() maxDate?: Date;
  @Input() showButtonBar: boolean = true;
  @Input() showTime: boolean = false;
  @Input() hourFormat: '12' | '24' = '12';
  @Input() disabledDates: Date[];
  @Input() disabledDays: number[];
  @Input() showSeconds: boolean = false;
  @Input() inputStyle: { [key: string]: string } = {};

  value: Date | null = null;
  onTouched: any = () => { };
  onChangeFn: any = (_: any) => { };

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
    if (this.required && (control.value === null || control.value === '')) {
      return { required: true };
    }
    return null;
  }

  onInputChange(event: any): void {
    this.value = event ? new Date(event) : null;
    this.onChangeFn(this.value);
  }

  onBlurEvent(): void {
    this.onTouched();
  }
}
