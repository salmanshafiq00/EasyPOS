import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { MenuItem } from 'primeng/api';

type SeverityType = 'success' | 'info' | 'warning' | 'primary' | 'help' | 'danger' | 'secondary' | 'contrast' | null;

@Component({
  selector: 'app-split-button',
  templateUrl: './split-button.component.html',
  styleUrls: ['./split-button.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SplitButtonComponent {
  @Input() label: string | null = null;
  @Input() showLabel: boolean = true;
  @Input() model: MenuItem[] = null;
  @Input() rowData: any;
  @Input() icon: string | null = null;
  @Input() dropdownIcon: string | null = null;
  @Input() iconPos: 'left' | 'right' = 'left';
  @Input() disabled: boolean = false;
  @Input() raised: boolean = false;
  @Input() rounded: boolean = false;
  @Input() text: boolean = false;
  @Input() plain: boolean = false;
  @Input() severity: SeverityType = 'primary';
  @Input() outlined: boolean = false;
  @Input() link: boolean = false;
  @Input() tabindex: number | null = null;
  @Input() size: 'small' | 'large' | null = null;
  @Input() style: any = null;
  @Input() styleClass: string | null = null;
  @Input() menuStyle: {} = null;
  @Input() menuStyleClass: string = 'menu-left-margin';
  @Input() menuButtonDisabled: boolean = null;
  @Input() buttonDisabled: boolean = null;
  @Input() dir: string = 'right';
  @Input() showTransitionOptions: string = '.12s cubic-bezier(0, 0, 0.2, 1)';
  @Input() hideTransitionOptions: string = '.1s linear';
  @Input() appendTo: any = null;
  @Input() ariaLabel: string | null = null;
  @Input() autofocus: boolean = false;
  @Input() tooltip: string = null;
  @Input() tooltipOptions: string = null;

  @Output() onClick = new EventEmitter<MouseEvent>();
  @Output() onFocus = new EventEmitter<FocusEvent>();
  @Output() onBlur = new EventEmitter<FocusEvent>();
  @Output() onMenuHide = new EventEmitter<any>();
  @Output() onMenuShow = new EventEmitter<any>();
  @Output() onDropdownClick = new EventEmitter<MouseEvent>();

  get asSeverity(): SeverityType {
    return this.severity as SeverityType;
  }

  handleClick(event: MouseEvent) {
    this.onClick.emit(event);
  }

  handleFocus(event: FocusEvent) {
    this.onFocus.emit(event);
  }

  handleBlur(event: FocusEvent) {
    this.onBlur.emit(event);
  }

  handleMenuHide(event: any) {
    this.onMenuHide.emit(event);
  }

  handleMenuShow(event: any) {
    if (this.onMenuShow) {
      this.onMenuShow.emit(event);
    }
  }
  
  handleDropdownClick(event: MouseEvent) {
    console.log(event)
    console.log(this.rowData)
    this.onDropdownClick.emit(event);
  }
}
