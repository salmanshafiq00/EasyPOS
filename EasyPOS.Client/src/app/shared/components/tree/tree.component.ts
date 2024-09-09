import { Component, EventEmitter, Input, Output, forwardRef, OnChanges, SimpleChanges } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { TreeNode } from 'primeng/api';

@Component({
  selector: 'app-tree',
  templateUrl: './tree.component.html',
  styleUrls: ['./tree.component.scss'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => TreeComponent),
    multi: true
  }]
})
export class TreeComponent implements ControlValueAccessor, OnChanges {
  @Input() label: string = '';
  @Input() required: boolean = false;
  @Input() disabled: boolean = false;
  @Input() readonly: boolean = false;
  @Input() hidden: boolean = false;
  @Input() autofocus: boolean = false;
  @Input() inputId: string = 'integeronly';
  @Input() options: TreeNode[] = [];
  @Input() selectionMode: 'single' | 'multiple' | 'checkbox' = 'checkbox';
  @Input() loadingMode: 'mask' | 'icon' = 'mask';
  @Input() selection: TreeNode[] | TreeNode | null = null; // Updated type to support single selection
  @Input() style: any = null;
  @Input() styleClass: string | null = null;
  @Input() contextMenu: any = null;
  @Input() layout: 'vertical' | 'horizontal' = 'vertical';
  @Input() metaKeySelection: boolean = false;
  @Input() propagateSelectionUp: boolean = true;
  @Input() propagateSelectionDown: boolean = true;
  @Input() loading: boolean = false;
  @Input() loadingIcon: string | null = null;
  @Input() emptyMessage: string | null = null;
  @Input() validateDrop: boolean = false;
  @Input() filter: boolean = false;
  @Input() filterBy: string = 'label';
  @Input() filterMode: 'lenient' | 'strict' = 'lenient';
  @Input() filterPlaceholder: string | null = null;
  @Input() filteredNodes: TreeNode[] | null = null;
  @Input() filterLocale: string | null = null;
  @Input() scrollHeight: string | null = null;
  @Input() lazy: boolean = false;
  @Input() virtualScroll: boolean = false;
  @Input() virtualScrollItemSize: number | null = null;
  @Input() virtualScrollOptions: any = null;
  @Input() indentation: number = 1.5;
  @Input() templateMap: any = null;
  @Input() trackBy: (index: number, item: TreeNode) => any = (index, item) => item;
  @Input() getset: 'key' | 'object' = 'key';

  @Output() selectionChange = new EventEmitter<TreeNode | TreeNode[]>();
  @Output() onNodeSelect = new EventEmitter<any>();
  @Output() onNodeUnselect = new EventEmitter<any>();
  @Output() onNodeExpand = new EventEmitter<any>();
  @Output() onNodeCollapse = new EventEmitter<any>();
  @Output() onNodeContextMenuSelect = new EventEmitter<any>();
  @Output() onLazyLoad = new EventEmitter<any>();
  @Output() onScroll = new EventEmitter<any>();
  @Output() onScrollIndexChange = new EventEmitter<any>();
  @Output() onFilter = new EventEmitter<any>();

  private onChange: any = () => {};
  private onTouched: any = () => {};

  private newValue: any[] = [];
  private oldValue: any;

  writeValue(value: any): void {
    this.oldValue = value;
    this.updateSelection();
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

  ngOnChanges(changes: SimpleChanges): void {
    if (changes?.['options']) {
      this.updateSelection();
    }
  }

  private updateSelection(): void {
    this.newValue = [];
    if (this.getset === 'key') {
      if (this.selectionMode === 'single' && typeof this.oldValue === 'string') {
        this.selectSingleNode(this.options, this.oldValue); // Added for single selection
        this.selection = this.newValue.length > 0 ? this.newValue[0] : null;
        this.onChange(this.newValue.length > 0 ? this.newValue[0].key : null);
      } else {
        this.selectNodes(this.options, this.oldValue);
        this.updateParentSelection(this.options, this.newValue);
        this.selection = this.newValue;
        this.onChange(this.newValue.filter(node => node.leaf).map(node => node.key));
      }
    } else {
      if (this.selectionMode === 'single' && typeof this.oldValue === 'object') {
        this.selectSingleNode(this.options, this.oldValue); // Added for single selection
        this.selection = this.newValue.length > 0 ? this.newValue[0] : null;
        this.onChange(this.newValue.length > 0 ? this.newValue[0] : null);
      } else {
        this.selectNodes(this.options, this.oldValue);
        this.updateParentSelection(this.options, this.newValue);
        this.selection = this.newValue;
        this.onChange(this.newValue);
      }
    }
  }

  onSelectionChange(event: any): void {
    this.newValue = event;
    if (this.getset === 'key') {
      if (this.selectionMode === 'single') {
        this.onChange(event ? event.key : null);
      } else {
        this.onChange(event?.filter((node: TreeNode) => node.leaf)?.map((node: TreeNode) => node.key) ?? []);
      }
    } else {
      if (this.selectionMode === 'single') {
        this.onChange(event);
      } else {
        this.onChange(event);
      }
    }
    this.selectionChange.emit(event);
  }

  private selectNodes(nodes: TreeNode[], selectedKeys: string[]) {
    nodes?.forEach(node => {
      if (selectedKeys?.includes(node.key!)) {  // Ensure key is not undefined
        this.newValue.push(node);
      }
      if (node.children) {
        this.selectNodes(node.children, selectedKeys);
      }
    });
  }

  private selectSingleNode(nodes: TreeNode[], selectedKey: string) {
    nodes?.forEach(node => {
      if (node.key === selectedKey) {
        this.newValue.push(node);
      }
      if (node.children) {
        this.selectSingleNode(node.children, selectedKey);
      }
    });
  }

  private updateParentSelection(nodes: TreeNode[], selectedNodes: TreeNode[]) {
    nodes?.forEach(node => {
      if (node.children && node.children.length > 0) {
        this.updateParentSelection(node.children, selectedNodes);  // Recursively update child nodes
        const totalChildren = node.children.length;
        const selectedChildren = node.children.filter(child => selectedNodes.includes(child)).length;
        if (selectedChildren === totalChildren) {
          // All children are selected, mark parent as selected
          if (!selectedNodes.includes(node)) {
            selectedNodes.push(node);
          }
          node.partialSelected = false;
        } else if (selectedChildren > 0) {
          // Some children are selected, mark parent as partially selected
          node.partialSelected = true;
          // Ensure parent is not fully selected if partially selected
          const parentIndex = selectedNodes.indexOf(node);
          if (parentIndex !== -1) {
            selectedNodes.splice(parentIndex, 1);
          }
        } else {
          // No children are selected, ensure parent is not selected
          node.partialSelected = false;
          const parentIndex = selectedNodes.indexOf(node);
          if (parentIndex !== -1) {
            selectedNodes.splice(parentIndex, 1);
          }
        }
      }
    });
  }
}
