import { Component, EventEmitter, Input, Output } from '@angular/core';

type SeverityType = 'success' | 'info' | 'warning' | 'primary' | 'help' | 'danger' | 'secondary' | 'contrast' | null;

@Component({
  selector: 'app-button',
  templateUrl: './button.component.html',
  styleUrl: './button.component.scss'
})
export class ButtonComponent {
  @Input() label: string | null = null;
  @Input() showLabel: boolean = true;
  @Input() icon: string | null = null;
  @Input() iconPos: 'left' | 'right' | 'top' | 'bottom' = 'left';
  @Input() type: string = 'button';
  @Input() badge: string | null = null;
  @Input() disabled: boolean = false;
  @Input() loading: boolean = false;
  @Input() loadingIcon: string | null = null;
  @Input() raised: boolean = false;
  @Input() rounded: boolean = false;
  @Input() text: boolean = false;
  @Input() plain: boolean = false;
  @Input() severity: string = 'primary';
  @Input() outlined: boolean = false;
  @Input() link: boolean = false;
  @Input() tabindex: number | null = null;
  @Input() size: 'small' | 'large' | null = null;
  @Input() style: any = null;
  @Input() styleClass: string | null = null;
  @Input() badgeClass: string | null = null;
  @Input() ariaLabel: string | null = null;
  @Input() autofocus: boolean = false;
  @Input() tooltip: string = '';
  @Input() tooltipPos: string = 'top';

  @Output() onClick = new EventEmitter<MouseEvent>();
  @Output() onFocus = new EventEmitter<FocusEvent>();
  @Output() onBlur = new EventEmitter<FocusEvent>();

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
}

