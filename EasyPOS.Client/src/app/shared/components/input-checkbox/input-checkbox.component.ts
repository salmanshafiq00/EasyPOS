import { Component, Input, forwardRef, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { NG_VALUE_ACCESSOR, NG_VALIDATORS, ControlValueAccessor, Validator, AbstractControl, ValidationErrors } from '@angular/forms';

@Component({
  selector: 'app-input-checkbox',
  templateUrl: './input-checkbox.component.html',
  styleUrls: ['./input-checkbox.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputCheckboxComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => InputCheckboxComponent),
      multi: true
    }
  ]
})
export class InputCheckboxComponent implements ControlValueAccessor, Validator, OnChanges {
  @Input() label: string = '';
  @Input() required: boolean = false;
  @Input() disabled: boolean = false;
  @Input() readonly: boolean = false;
  @Input() hidden: boolean = false;
  @Input() autofocus: boolean = false;
  @Input() trueValue: any = true;
  @Input() falseValue: any = false;
  @Input() binary: boolean = false;
  @Input() name: string;
  @Input() column: boolean = false;
  @Input() getset: 'key' | 'object' = 'object';
  @Input() variant: 'outlined' | 'filled' = 'outlined';
  @Input() options: any[] = [];
  @Input() checkedIcon: string = 'pi pi-check';
  @Input() uncheckedIcon: string = 'pi pi-times';

  value: any[] = [];
  initialValue: any[] = [];
  selectedOptions: any[] = [];
  onTouched: any = () => { };
  onChangeFn: any = (_: any) => { };

  ngOnChanges(changes: SimpleChanges): void {
    if (changes?.['options'] && !changes?.['options'].isFirstChange()) {
      this.updateSelection(this.initialValue);
    }
  }

  writeValue(value: any[]): void {
    this.initialValue = value || [];
    this.updateSelection(this.initialValue);
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
    if (this.getset === 'key') {
      this.value = event.checked ? event.checked.map((x: any) => x.id) : [];
    } else {
      this.value = event.checked || [];
    }
    this.onChangeFn(this.value);
  }
  

  onBlurEvent(): void {
    this.onTouched();
  }

  private updateSelection(value: any[]): void {
      this.selectedOptions = this.options?.filter(option => value.includes(option.id)) || [];
      if(this.getset === 'object'){
        this.value = this.selectedOptions;
        this.onChangeFn(this.value);  
      }
  }
}
