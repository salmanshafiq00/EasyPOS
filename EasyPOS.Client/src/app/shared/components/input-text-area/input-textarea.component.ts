import { Component, EventEmitter, Input, Output } from '@angular/core';
import { AbstractControl, ControlValueAccessor, NG_VALIDATORS, NG_VALUE_ACCESSOR, ValidationErrors, Validator } from '@angular/forms';

@Component({
  selector: 'app-input-textarea',
  templateUrl: './input-textarea.component.html',
  styleUrl: './input-textarea.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting:  InputTextAreaComponent,
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting:  InputTextAreaComponent,
      multi: true
    }
  ]
})
export class InputTextAreaComponent implements ControlValueAccessor, Validator {
  @Input() label: string = '';
  @Input() placeholder: string = '';
  @Input() required: boolean = false;
  @Input() disabled: boolean = false;
  @Input() readonly: boolean = false;
  @Input() hidden: boolean = false;
  @Input() variant: string = 'outlined';
  @Input() autofocus: boolean = false;
  @Input() rows: number = 3;
  @Input() cols: number = 30;
  @Input() autoResize: boolean = false;
  @Input() max: number;
  @Input() showCharLength: boolean = false;
  @Input() width: string = '';
  @Input() height: string = '';
  @Output() onblur = new EventEmitter<string>();

  value: string = '';
  onTouched: any = () => {};
  onChangeFn: any = (_: any) => {};

  writeValue(value: any): void {
    this.value = value || '';
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

  ngAfterViewInit(): void {
    if (this.value) {
      this.onChangeFn(this.value);
    }
  }

  onInputChange(event: any): void {
    this.value = event.target.value;
    this.onChangeFn(this.value);
  }

  onBlurEvent(): void {
    this.onTouched();
    this.onblur.emit(this.value)
  }


}
