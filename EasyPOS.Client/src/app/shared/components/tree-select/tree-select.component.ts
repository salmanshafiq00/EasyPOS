import { Component, Input, forwardRef, OnChanges, SimpleChanges } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR, NG_VALIDATORS, ValidationErrors, AbstractControl } from '@angular/forms';
import { TreeNode } from 'primeng/api';

@Component({
  selector: 'app-tree-select',
  templateUrl: './tree-select.component.html',
  styleUrls: ['./tree-select.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => TreeSelectComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => TreeSelectComponent),
      multi: true
    }
  ]
})
export class TreeSelectComponent implements ControlValueAccessor, OnChanges {
  @Input() label: string = '';
  @Input() required: boolean = false;
  @Input() disabled: boolean = false;
  @Input() hidden: boolean = false;
  @Input() placeholder: string = 'Select One';
  @Input() autofocus: boolean = false;
  @Input() variant: 'outlined' | 'filled' = 'outlined';
  @Input() options: TreeNode[] = [];
  @Input() inputId: string = '';
  @Input() scrollHeight: string = '400px';
  @Input() metaKeySelection: boolean = false;
  @Input() display: 'comma' | 'chip' = 'comma';
  @Input() selectionMode: 'single' | 'multiple' | 'checkbox' = 'single';
  @Input() tabindex: string = '0';
  @Input() panelClass: string | string[] | Set<string> | Object = '';
  @Input() panelStyle: Object = {};
  @Input() panelStyleClass: string = '';
  @Input() containerStyle: Object = {};
  @Input() containerStyleClass: string = '';
  @Input() labelStyle: Object = {};
  @Input() labelStyleClass: string = '';
  @Input() overlayOptions: any = null;
  @Input() emptyMessage: string = '';
  @Input() appendTo: any = null;
  @Input() filter: boolean = true;
  @Input() filterBy: string = 'label';
  @Input() filterMode: string = 'lenient';
  @Input() filterPlaceholder: string = '';
  @Input() filterInputAutoFocus: boolean = true;
  @Input() propagateSelectionDown: boolean = true;
  @Input() propagateSelectionUp: boolean = true;
  @Input() showClear: boolean = true;
  @Input() resetFilterOnHide: boolean = true;
  @Input() loading: boolean = false;
  @Input() getset: 'key' | 'object' = 'key';

  value: TreeNode | TreeNode[] | string | string[] | null = null;
  onTouched: any = () => {};
  onChangeFn: any = (_: any) => {};
  private internalValue: any;

  writeValue(value: any): void {
    this.internalValue = value;
    this.updateSelection();
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

  ngOnChanges(changes: SimpleChanges): void {
    if (changes?.['options']) {
      this.updateSelection();
    }
  }

  validate(control: AbstractControl): ValidationErrors | null {
    return this.required && (!this.value || (Array.isArray(this.value) && this.value.length === 0)) ? { required: true } : null;
  }

  onFocus(event: any): void {
    this.onTouched();
  }

  onBlur(event: any): void {
    this.onTouched();
  }

  onNodeSelect(event: any): void {
    if (this.selectionMode === 'single') {
      this.value = event.node;
    } else {
      this.value = [...(this.value as TreeNode[]), event.node];
    }
    this.propagateChange();
  }

  onNodeUnselect(event: any): void {
    if (this.selectionMode === 'single') {
      this.value = null;
    } else {
      this.value = (this.value as TreeNode[]).filter(node => node.key !== event.node.key);
    }
    this.propagateChange();
  }

  private updateSelection(): void {
    if (this.selectionMode === 'single') {
      this.value = this.getset === 'key' ? this.selectSingleNodeByKey(this.options, this.internalValue) : this.internalValue;
    } else {
      this.value = this.getset === 'key' ? this.selectNodesByKeys(this.options, this.internalValue) : this.internalValue;
    }
    this.propagateChange();
  }

  private propagateChange(): void {
    if (this.getset === 'key') {
      if (this.selectionMode === 'single') {
        this.onChangeFn((this.value as TreeNode)?.key);
      } else {
        this.onChangeFn((this.value as TreeNode[])?.map(node => node.key));
      }
    } else {
      this.onChangeFn(this.value);
    }
  }

  private selectNodesByKeys(nodes: TreeNode[], keys: string[]): TreeNode[] {
    let selectedNodes: TreeNode[] = [];
    nodes?.forEach(node => {
      if (keys.includes(node.key)) {
        selectedNodes.push(node);
      }
      if (node.children) {
        selectedNodes = [...selectedNodes, ...this.selectNodesByKeys(node.children, keys)];
      }
    });
    return selectedNodes;
  }

  private selectSingleNodeByKey(nodes: TreeNode[], key: string): TreeNode | null {
    let selectedNode: TreeNode | null = null;
    nodes?.forEach(node => {
      if (node.key === key) {
        selectedNode = node;
      }
      if (!selectedNode && node.children) {
        selectedNode = this.selectSingleNodeByKey(node.children, key);
      }
    });
    return selectedNode;
  }
}
