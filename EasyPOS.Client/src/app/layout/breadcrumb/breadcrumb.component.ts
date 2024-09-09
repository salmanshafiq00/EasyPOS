import { AfterViewInit, Component, OnInit } from '@angular/core';
import { AppMenuService } from '../sidebar/app-menu.service';
import { SidebarMenuModel } from 'src/app/modules/generated-clients/api-service';
import { Subscription, debounceTime, filter } from 'rxjs';
import { NavigationEnd, Router } from '@angular/router';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-breadcrumb',
  templateUrl: './breadcrumb.component.html',
  styleUrl: './breadcrumb.component.scss'
})
export class BreadcrumbComponent implements OnInit {
  breadcrumbs = [];
  appMenus: SidebarMenuModel[] = [];
  selectedMenu: any;
  subscriptions: Subscription = new Subscription();

  constructor(private appMenuService: AppMenuService,
    private router: Router
  ) { }

  ngOnInit() {
    this.subscriptions.add(this.appMenuService.getSidebarMenus().subscribe(data => {
      this.appMenus = data;
      this.updateBreadcrumbs();
    }));

    // this.subscriptions.add(this.appMenuService.getSelectedMenu().subscribe(data => {
    //   this.selectedMenu = data;
    //   this.updateBreadcrumbs();
    // }));

    this.subscriptions.add(this.router.events.pipe(filter(event => event instanceof NavigationEnd)).subscribe(event => {
      this.updateBreadcrumbs();
    }));

  }

  private updateBreadcrumbs() {
    const currentUrl = this.router.url;

    if(currentUrl === '/' || currentUrl === '/dashboard') {
      this.breadcrumbs = [];
      return;
    };

    const matchedMenu = this.findMenuByUrl(currentUrl);

    if (matchedMenu) {
      if (!this.selectedMenu || this.selectedMenu.id !== matchedMenu.id) {
        this.selectedMenu = matchedMenu;
      }
    } else {
      this.breadcrumbs = [];  // Clear breadcrumbs if no menu matches the URL
      this.breadcrumbs = this.getUnmatchedBreadcrumb(currentUrl);  
      return;
    }
    if (this.selectedMenu && this.appMenus.length > 0) {
      this.setBreadcrumb();
    }
  }

  private findMenuByUrl(url: string): SidebarMenuModel | undefined{
    const flatMenus = this.flattenMenus(this.appMenus);
    return flatMenus?.find(menu => menu?.routerLink[0]?.toLowerCase() === url?.toLowerCase());
  }

  private getUnmatchedBreadcrumb(currentUrl: string): any[]{
    const breadcrumb = currentUrl.split('/').map(x => ({label: x}));
    return breadcrumb;
  }

  private setBreadcrumb() {
    if (!this.selectedMenu) return;

    // Reset breadcrumbs
    this.breadcrumbs = [];

    // Adding the selected menu to breadcrumbs
    this.breadcrumbs.push({
      label: this.selectedMenu.label,
      route: this.selectedMenu.routerLink,
      icon: this.selectedMenu.icon
    });

    // Adding parent menus to breadcrumbs
    this.addParentMenu(this.selectedMenu.parentId);

    if(this.selectedMenu.routerLink !== '/'){
      this.breadcrumbs.unshift({
        route: '/',
        icon: 'pi pi-home'
      });
    }
  }

  private addParentMenu(parentId: string) {
    if (!parentId || parentId === '00000000-0000-0000-0000-000000000000') return;

    const parentMenu = this.appMenus.find(x => x.id === parentId);
    if (parentMenu) {
      this.breadcrumbs.unshift({
        label: parentMenu.label,
        // route: parentMenu.routerLink,
        icon: parentMenu.icon
      });

      // Recursively add ancestor menus
      this.addParentMenu(parentMenu.parentId);
    }
  }

  private flattenMenus(menus: SidebarMenuModel[]): SidebarMenuModel[] {
    let flatMenus: SidebarMenuModel[] = [];
    for (let menu of menus) {
      flatMenus.push(menu);
      if (menu.items && menu.items.length > 0) {
        flatMenus = flatMenus.concat(this.flattenMenus(menu.items));
      }
    }
    return flatMenus;
  }
  

  ngOnDestroy() {
    this.subscriptions.unsubscribe();
  }
}