import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { InputTextModule } from 'primeng/inputtext';
import { SidebarModule } from 'primeng/sidebar';
import { BadgeModule } from 'primeng/badge';
import { RadioButtonModule } from 'primeng/radiobutton';
import { InputSwitchModule } from 'primeng/inputswitch';
import { RippleModule } from 'primeng/ripple';
import { AppTopBarComponent } from './topbar/app.topbar.component';
import { AppFooterComponent } from './footer/app.footer.component';
import { AppConfigModule } from './config/config.module';
import { AppSidebarComponent } from "./sidebar/app.sidebar.component";
import { AppLayoutComponent } from "./app.layout.component";
import { RouterModule } from '@angular/router';
import { AppMenuitemComponent } from './sidebar/components/menu/app.menuitem.component';
import { AppMenuComponent } from './sidebar/components/menu/app.menu.component';
import { API_BASE_URL, AppMenusClient } from '../modules/generated-clients/api-service';
import { environment } from 'src/environments/environment';
import { BreadcrumbModule } from 'primeng/breadcrumb';
import { AppMenuService } from './sidebar/app-menu.service';
import { BreadcrumbComponent } from './breadcrumb/breadcrumb.component';
import { OverlayPanelModule } from 'primeng/overlaypanel';
import { ToastModule } from 'primeng/toast';
import { MenuModule } from 'primeng/menu';

@NgModule({
    declarations: [
        AppMenuitemComponent,
        AppTopBarComponent,
        AppFooterComponent,
        AppMenuComponent,
        AppSidebarComponent,
        AppLayoutComponent,
        BreadcrumbComponent
    ],
    imports: [
        BrowserModule,
        FormsModule,
        HttpClientModule,
        BrowserAnimationsModule,
        InputTextModule,
        SidebarModule,
        BadgeModule,
        RadioButtonModule,
        InputSwitchModule,
        RippleModule,
        AppConfigModule,
        RouterModule,
        BreadcrumbModule,
        OverlayPanelModule,
        ToastModule,
        MenuModule
    ],
    providers: [
        { provide: API_BASE_URL, useValue: environment.API_BASE_URL },
        AppMenusClient,
        AppMenuService
    ],
    exports: [AppLayoutComponent]
})
export class AppLayoutModule { }
