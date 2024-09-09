import { Component, Input, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR, NG_VALIDATORS, ControlValueAccessor, Validator, AbstractControl, ValidationErrors } from '@angular/forms';

@Component({
  selector: 'app-input-time',
  templateUrl: './input-time.component.html',
  styleUrls: ['./input-time.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputTimeComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => InputTimeComponent),
      multi: true
    }
  ]
})
export class InputTimeComponent implements ControlValueAccessor, Validator {
  @Input() label: string = '';
  @Input() placeholder: string = 'hh:mm';
  @Input() required: boolean = false;
  @Input() disabled: boolean = false;
  @Input() readonly: boolean = false;
  @Input() hidden: boolean = false;
  @Input() variant: 'outlined' | 'filled' = 'outlined';
  @Input() autofocus: boolean = false;
  @Input() showIcon: boolean = true;
  @Input() showOnFocus: boolean = true;
  @Input() iconDisplay: 'input' | 'button' = 'input';
  @Input() readonlyInput: boolean = false;
  @Input() timeOnly: boolean = true;
  @Input() hourFormat: '12' | '24' = '24';
  @Input() showButtonBar: boolean = true;
  @Input() showSeconds: boolean = false;
  @Input() dataType: 'string' | 'date' = 'string';
  @Input() inputStyle: { [key: string]: string } = {};

  value: Date | null = null;
  onTouched: any = () => { };
  onChangeFn: any = (_: any) => { };

  writeValue(value: any): void {
    if (value) {
      if (typeof value === 'string') {
        const [hours, minutes, seconds] = value.split(':').map(Number);
        this.value = new Date();
        this.value.setHours(hours);
        this.value.setMinutes(minutes);
        this.value.setSeconds(seconds || 0);
      } else {
        this.value = value;
      }
    } else {
      this.value = null;
    }
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
    if (event instanceof Date) {
      this.value = event;
    } else if (typeof event === 'string') {
      const [hours, minutes, seconds] = event.split(':').map(Number);
      this.value = new Date();
      this.value.setHours(hours);
      this.value.setMinutes(minutes);
      this.value.setSeconds(seconds || 0);
    } else {
      this.value = null;
    }

    if (this.value) {
      const timeString = this.value.toTimeString().split(' ')[0];
      this.onChangeFn(timeString);
    }
  }

  onBlurEvent(): void {
    this.onTouched();
  }
}
