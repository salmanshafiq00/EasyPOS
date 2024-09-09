import { Component, forwardRef, Input } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR, NG_VALIDATORS, Validator, AbstractControl, ValidationErrors } from '@angular/forms';

@Component({
  selector: 'app-input-mask',
  templateUrl: './input-mask.component.html',
  styleUrls: ['./input-mask.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputMaskComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => InputMaskComponent),
      multi: true
    }
  ]
})
export class InputMaskComponent implements ControlValueAccessor, Validator {
  @Input() label: string = '';
  @Input() placeholder: string = '';
  @Input() required: boolean = false;
  @Input() disabled: boolean = false;
  @Input() readonly: boolean = false;
  @Input() hidden: boolean = false;
  @Input() variant: 'outlined' | 'filled' = 'outlined';
  @Input() autofocus: boolean = false;
  @Input() inputId: string = null;
  @Input() type: string = 'text';
  @Input() mask: string = null;
  @Input() slotChar: string = '_';
  @Input() autoClear: boolean = true;
  @Input() showClear: boolean = true;
  @Input() style: any = null;
  @Input() styleClass: string = null;
  @Input() size: number = null;
  @Input() maxlength: number = null;
  @Input() title: string = null;
  @Input() unmask: boolean = false;
  @Input() name: string = null;
  @Input() autocomplete: string = null;
  @Input() characterPattern: string = '[A-Za-z]';

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
    this.value = event.target.value;
    this.onChangeFn(this.value);
  }

  onBlurEvent(): void {
    this.onChangeFn(this.value);
    this.onTouched();
  }
}
