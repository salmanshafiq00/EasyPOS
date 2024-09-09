import { Component, Input, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR, NG_VALIDATORS, ControlValueAccessor, Validator, AbstractControl, ValidationErrors } from '@angular/forms';

@Component({
  selector: 'app-input-decimal',
  templateUrl: './input-decimal.component.html',
  styleUrls: ['./input-decimal.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputDecimalComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => InputDecimalComponent),
      multi: true
    }
  ]
})
export class InputDecimalComponent implements ControlValueAccessor, Validator {
  @Input() label: string = '';
  @Input() placeholder: string = '';
  @Input() required: boolean = false;
  @Input() disabled: boolean = false;
  @Input() readonly: boolean = false;
  @Input() hidden: boolean = false;
  @Input() variant: 'outlined' | 'filled' = 'outlined';
  @Input() autofocus: boolean = false;
  @Input() inputId: string = '';
  @Input() min: number = null;
  @Input() max: number = null;
  @Input() textRight: boolean = false;
  @Input() minFractionDigits: number = 2;
  @Input() maxFractionDigits: number = 2;
  @Input() mode: string = 'decimal';
  @Input() locale: string = 'en-US';
  @Input() prefix: string = '';
  @Input() suffix: string = '';
  @Input() showButtons: boolean = false;
  @Input() buttonLayout: string = 'stacked';
  // @Input() spinnerMode: string = 'stacked';
  @Input() useGrouping: boolean = false;
  @Input() step: number = 1;
  @Input() decrementButtonClass: string = 'p-button-danger';
  @Input() incrementButtonClass: string = 'p-button-success';
  @Input() incrementButtonIcon: string = 'pi pi-plus';
  @Input() decrementButtonIcon: string = 'pi pi-minus';

  value: number | null = null;
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
    if (this.required && (control.value === null || control.value === '')) {
      return { required: true };
    }
    return null;
  }

  onInputChange(event: any): void {
    this.value = event ? parseFloat(event) : null;
    this.onChangeFn(this.value);
  }
  

  onBlurEvent(): void {
    this.onTouched();
  }
}
