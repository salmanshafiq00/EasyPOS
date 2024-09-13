import { OnInit } from '@angular/core';
import { Component } from '@angular/core';
import { LayoutService } from '../../../service/app.layout.service';
import { AppMenusClient, SidebarMenuModel } from 'src/app/modules/generated-clients/api-service';
import { AppMenuService } from '../../app-menu.service';
import { PermissionService } from 'src/app/core/auth/services/permission.service';
import { NotificationService } from 'src/app/core/services/notification.service';

@Component({
    selector: 'app-menu',
    templateUrl: './app.menu.component.html'

})
export class AppMenuComponent implements OnInit {

    model: any[] = [];

    constructor(public layoutService: LayoutService,
        private appMenuClient: AppMenusClient,
        private appMenuService: AppMenuService,
        private permissionService: PermissionService,
        private signalrNotificationService: NotificationService
    ) { 

        this.signalrNotificationService.permissionChanged.subscribe({
            next: () => {
                setTimeout(() => {
                    this.permissionService.loadPermissions(false);    
                }, 5000);
            }
        });
    }

    ngOnInit() {

        this.appMenuClient.getSidebarMenus().subscribe({
            next: (res) => {
                const transformedMenu = this.transformMenuData(res);
                this.model.push({
                    items: transformedMenu
                });
                this.appMenuService.setSidebarMenus(transformedMenu);
            },
            error: (error) => {
                console.log(error);
            }
        });

        this.permissionService.loadPermissions(true);
    }

    private transformMenuData(data: SidebarMenuModel[]): any[] {
        return data.map(item => this.mapMenuItem(item));
    }

    private mapMenuItem(item: SidebarMenuModel): any {

        if (item.items && item.items.length > 0) {
            return {
                id: item.id,
                label: item.label,
                icon: item.icon,
                routerLink: [item.routerLink],
                parentId: item.parentId,
                parentLabel: item.parentLabel,
                items: item.items.map(child => this.mapMenuItem(child))
            };

        } else {
            return {
                id: item.id,
                label: item.label,
                icon: item.icon,
                routerLink: [item.routerLink],
                parentId: item.parentId,
                parentLabel: item.parentLabel,
            };

        }

    }
}
