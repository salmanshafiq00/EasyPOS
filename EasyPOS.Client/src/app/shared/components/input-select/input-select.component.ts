import { Component, EventEmitter, Input, Output, forwardRef } from '@angular/core';
import { AbstractControl, ControlValueAccessor, NG_VALIDATORS, NG_VALUE_ACCESSOR, ValidationErrors, Validator } from '@angular/forms';

@Component({
  selector: 'app-input-select',
  templateUrl: './input-select.component.html',
  styleUrl: './input-select.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputSelectComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => InputSelectComponent),
      multi: true
    }
  ]
})
export class InputSelectComponent implements ControlValueAccessor, Validator {
  @Input() label: string = '';
  @Input() required: boolean = false;
  @Input() disabled: boolean = false;
  @Input() readonly: boolean = false;
  @Input() hidden: boolean = false;
  @Input() placeholder: string = 'Select One';
  @Input() autofocus: boolean = false;
  @Input() variant: 'outlined' | 'filled' = 'outlined';
  @Input() options: any[] = [];
  @Input() inputId: string = '';
  @Input() checkmark: boolean = true;
  @Input() showClear: boolean = true;
  @Input() editable: boolean = false;
  @Input() filter: boolean = true;
  @Input() filterBy: string = 'name';
  @Input() optionLabel: string = 'name';
  @Input() optionValue: string = 'id';
  @Output() onChange: EventEmitter<any> = new EventEmitter<any>();
  // @Input() emptyMessage: string = 'No Data Found!';

  value: any = null;
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
    return this.required && (!this.value || this.value.length === 0) ? { required: true } : null;
  }

  onInputChange(event: any): void {
    this.value = event.value;
    this.onChangeFn(this.value);
    this.onChange.emit(event.value);
  }
  

  onBlurEvent(): void {
    this.onTouched();
  }
}

