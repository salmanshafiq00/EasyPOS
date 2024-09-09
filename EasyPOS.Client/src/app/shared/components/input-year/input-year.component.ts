import { Component, forwardRef, Input } from "@angular/core";
import { NG_VALUE_ACCESSOR, NG_VALIDATORS, ControlValueAccessor, Validator, AbstractControl, ValidationErrors } from "@angular/forms";

@Component({
  selector: 'app-input-year',
  templateUrl: './input-year.component.html',
  styleUrls: ['./input-year.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputYearComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => InputYearComponent),
      multi: true
    }
  ]
})
export class InputYearComponent implements ControlValueAccessor, Validator {
  @Input() label: string = '';
  @Input() placeholder: string = 'year';
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
  @Input() view: 'date' | 'month' | 'year' = 'year';
  @Input() dateFormat: string = 'yy';
  @Input() inputStyle: { [key: string]: string } = {};

  value: Date | null = null;
  onTouched: any = () => { };
  onChangeFn: any = (_: any) => { };

  writeValue(value: any): void {
    if (value) {
      this.value = new Date();
      this.value.setFullYear(value);
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
    if (this.value) {
      const year = this.value.getFullYear();
      this.onChangeFn(year);
    }
  }

  onBlurEvent(): void {
    this.onTouched();
  }
}
