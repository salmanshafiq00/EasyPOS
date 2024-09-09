import { Component, Input, SimpleChanges, forwardRef, OnChanges } from '@angular/core';
import { NG_VALUE_ACCESSOR, NG_VALIDATORS, ControlValueAccessor, Validator, AbstractControl, ValidationErrors } from '@angular/forms';

@Component({
  selector: 'app-input-radio',
  templateUrl: './input-radio.component.html',
  styleUrls: ['./input-radio.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputRadioComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => InputRadioComponent),
      multi: true
    }
  ]
})
export class InputRadioComponent implements ControlValueAccessor, Validator, OnChanges {
  @Input() label: string = '';
  @Input() required: boolean = false;
  @Input() disabled: boolean = false;
  @Input() hidden: boolean = false;
  @Input() autofocus: boolean = false;
  @Input() name: string;
  @Input() column: boolean = false;
  @Input() inputId: string = null;
  @Input() style: Object = null;
  @Input() styleClass: string = null;
  @Input() labelStyleClass: string = null;
  @Input() getset: 'key'| 'object' = 'key';
  @Input() tabindex: number = null;
  @Input() variant: 'outlined' | 'filled' = 'outlined';
  @Input() options: any[] = [];

  value: any;
  initalValue: any;
  private onTouched: () => void = () => {};
  private onChange: (value: any) => void = () => {};

  ngOnChanges(changes: SimpleChanges): void {
    if (changes?.['options']) {
      this.updateValue(this.initalValue);
    }
  }

  writeValue(value: any): void {
    this.initalValue = value;
    this.updateValue(value);
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
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

  private updateValue(value: any): void {
    if (this.getset === 'key') {
      this.value = this.options?.find(option => option.id === value);
    } else {
      this.value = value;
    }
  }

  onInputChange(event: any): void {
    const selectedValue = event.value;
    this.value = this.getset === 'key' ? selectedValue?.id : selectedValue;
    this.onChange(this.value);
  }

  onBlur(): void {
    this.onTouched();
  }
}
