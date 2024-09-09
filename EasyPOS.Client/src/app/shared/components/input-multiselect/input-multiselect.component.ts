import { Component, Input, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR, NG_VALIDATORS, ControlValueAccessor, Validator, AbstractControl, ValidationErrors } from '@angular/forms';

@Component({
  selector: 'app-input-multiselect',
  templateUrl: './input-multiselect.component.html',
  styleUrl: './input-multiselect.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputMultiselectComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => InputMultiselectComponent),
      multi: true
    }
  ]
})
export class InputMultiselectComponent implements ControlValueAccessor, Validator {
  @Input() label: string = '';
  @Input() required: boolean = false;
  @Input() disabled: boolean = false;
  @Input() readonly: boolean = false;
  @Input() hidden: boolean = false;
  @Input() autofocus: boolean = false;
  @Input() placeholder: string = 'Select Any';
  @Input() filter: boolean = true;
  @Input() filterPlaceHolder: string = null;
  @Input() overlayVisible: boolean = true;
  @Input() variant: 'outlined' | 'filled' = 'outlined';
  @Input() appendTo: any = null;
  @Input() name: string = null;
  @Input() options: any[] = [];
  @Input() showClear: boolean = false;
  @Input() filterBy: string = 'name';
  @Input() optionLabel: string = 'name';
  @Input() optionValue: string = 'id';
  @Input() scrollHeight: string = '200px';
  @Input() selectionLimit: number = null;
  @Input() maxSelectedLabels: number = null;
  @Input() filterMatchMode: 'endsWith' | 'startsWith' | 'contains' | 'equals' | 'notEquals' | 'in' | 'lt' | 'lte' | 'gt' | 'gte' = 'contains';
  @Input() display: 'comma' | 'chip' = 'chip';
  @Input() autocomplete: string = 'on';
  @Input() dropdownIcon: string = null;
  @Input() selectedItemsLabel: string = 'ellipsis';
  @Input() displaySelectedLabel: boolean = true;
  @Input() showAllItems: boolean = false;
  @Input() tooltip: string = null;
  @Input() showTooltip: boolean = true;
  @Input() tooltipPosition: "left" | "top" | "bottom" | "right"	 = 'right';

  value: any[] = [];
  onTouched: any = () => { };
  onChangeFn: any = (_: any) => { };

  writeValue(value: any[]): void {
    this.value = value ? value : [];
    if(this.showTooltip){
      this.tooltip = this.options?.filter(x => this.value?.includes(x.id))?.map(x => x.name)?.join(', ') || null;
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
    return this.required && (!this.value || this.value.length === 0) ? { required: true } : null;
  }

  onInputChange(event: any): void {
    this.value = event.value;
    this.onChangeFn(this.value);
    if(this.showTooltip){
      this.tooltip = this.options?.filter(x => this.value?.includes(x.id))?.map(x => x.name)?.join(', ') || null;
    }  
  }
  

  onBlurEvent(): void {
    this.onTouched();
  }
}
