import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { MenuItem } from 'primeng/api';

//type SeverityType = 'success' | 'info' | 'warning' | 'primary' | 'help' | 'danger' | 'secondary' | 'contrast' | null;

@Component({
  selector: 'app-custom-menu',
  templateUrl: './custom-menu.component.html',
  styleUrl: './custom-menu.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CustomMenuComponent {
  @Input() items: MenuItem[] = null;
  @Input() data: any;

  // button configured
  @Input() label: string | null = null;
  @Input() showLabel: boolean = true;
  @Input() icon: string | null = 'pi pi-chevron-down';
  @Input() disabled: boolean = false;
  @Input() raised: boolean = false;
  @Input() rounded: boolean = false;
  @Input() text: boolean = false;
  @Input() plain: boolean = false;
  @Input() severity: string = 'primary';
  @Input() iconPos: "left" | "right" | "top" | "bottom" = 'right';
  @Input() outlined: boolean = false;
  @Input() link: boolean = false;
  @Input() tabindex: number | null = null;
  @Input() size: 'small' | 'large' | null = null;
  @Input() style: any = null;
  @Input() styleClass: string | null = null;

  // Menu Configuration
  @Input() popup: boolean = true;
  @Input() appendTo: string = 'body';
  @Input() showStartTemplate: boolean = false;
  @Input() showEndTemplate: boolean = false;
  @Input() showSubMenuHeader: boolean = false;

  @Output() onMenuClick = new EventEmitter<any>();

  // ngOnInit() {
  //   this.items = [
  //     {
  //       label: 'Edit',
  //       icon: 'pi pi-pen-to-square',
  //       menuStyle: 'primary'
  //     },
  //     {
  //       label: 'Delete',
  //       icon: 'pi pi-trash',
  //       menuStyle: 'danger'
  //     },
  //   ];

  //   this.items = [
  //     {
  //       label: 'New',
  //       icon: 'pi pi-plus',
  //       shortcut: '⌘+N'
  //     },
  //     {
  //       label: 'Search',
  //       icon: 'pi pi-search',
  //       shortcut: '⌘+S'
  //     }
  //   ]
  // }

  handleMenuClick(item: MenuItem, data: any){
    // console.log(item)
    // console.log(data)
    this.onMenuClick.emit({menuItem: item, data: data})
  }
}
