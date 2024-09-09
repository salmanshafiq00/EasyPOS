import { Component, Input, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR, NG_VALIDATORS, ControlValueAccessor, Validator, AbstractControl, ValidationErrors } from '@angular/forms';

@Component({
  selector: 'app-input-switch',
  templateUrl: './input-switch.component.html',
  styleUrls: ['./input-switch.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputSwitchComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => InputSwitchComponent),
      multi: true
    }
  ]
})
export class InputSwitchComponent implements ControlValueAccessor, Validator {
  @Input() label: string = '';
  @Input() disabled: boolean = false;
  @Input() readonly: boolean = false;
  @Input() hidden: boolean = false;
  @Input() autofocus: boolean = false;
  @Input() trueValue: any = true;
  @Input() falseValue: any = false;
  @Input() labelPosition: 'left' | 'top' = 'left';
  value: boolean = false;
  onTouched: any = () => { };
  onChangeFn: any = (_: any) => { };

  get isLeftPosition(): boolean {
    return this.labelPosition === 'left';
  }

  writeValue(value: any): void {
    this.value = value !== null ? value : false;
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
    return null;
  }

  onInputChange(event: any): void {
    this.value = event.checked;
    this.onChangeFn(this.value);
  }

  onBlurEvent(): void {
    this.onTouched();
  }
}
