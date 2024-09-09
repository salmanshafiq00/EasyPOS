import { Component, Input } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR, NG_VALIDATORS, AbstractControl, ValidationErrors, Validator, ControlContainer } from '@angular/forms';

@Component({
  selector: 'app-input-color',
  templateUrl: './input-color.component.html',
  styleUrls: ['./input-color.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting:  InputColorComponent,
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting:  InputColorComponent,
      multi: true
    }
  ]
})
export class InputColorComponent implements ControlValueAccessor, Validator {
  @Input() label: string = '';
  @Input() required: boolean = false;
  @Input() disabled: boolean = false;
  @Input() readonly: boolean = false;
  @Input() hidden: boolean = false;
  @Input() variant: 'outlined' | 'filled' = 'outlined';
  @Input() autofocus: boolean = false;
  @Input() inline: boolean = false;
  @Input() format: 'rgb' | 'hex' | 'hsb' = 'hex';
  @Input() appendTo: string = null;
  @Input() inputId: string = null;
  @Input() labelPosition: 'left' | 'top' = 'left';

  value: string;
  onTouched: any = () => {};
  onChangeFn: any = (_: any) => {};

  get isLeftPosition(): boolean {
    return this.labelPosition === 'left';
  }

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
    this.value = event.value;
    this.onChangeFn(this.value);
  }

  onBlurEvent(): void {
    this.onTouched();
  }
}