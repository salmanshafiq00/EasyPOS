import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MenuItem, MessageService } from 'primeng/api';
import { LayoutService } from "../service/app.layout.service";
import { OverlayPanel } from 'primeng/overlaypanel';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { AppNotificationModel } from 'src/app/modules/generated-clients/api-service';
import { NotificationService } from 'src/app/core/services/notification.service';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/core/auth/services/auth.service';

@Component({
    selector: 'app-topbar',
    templateUrl: './app.topbar.component.html',
    styleUrls: ['./app.topbar.component.scss']
})
export class AppTopBarComponent implements OnInit {

    profileDrawerItems!: MenuItem[];

    @ViewChild('menubutton') menuButton!: ElementRef;
    @ViewChild('topbarmenubutton') topbarMenuButton!: ElementRef;
    @ViewChild('topbarmenu') menu!: ElementRef;
    @ViewChild('notifPanel') notifPanel!: OverlayPanel;

    initialLoaded = false;
    notifications: AppNotificationModel[] = [];
    unseenMsgCount = 0;

    private readonly apiUrl = `${environment.API_BASE_URL}/api/AppNotifications/GetByUser`;
    private readonly httpOptions = {
        observe: 'body' as const,
        withCredentials: true,
        headers: new HttpHeaders({
            'Accept': 'application/json'
        })
    };

    get userPhotoUrl(){
        const userPhotoUrl = localStorage.getItem('userPhotoUrl') || this.authService.getPhotoUrl();
        return userPhotoUrl ? `${environment.API_BASE_URL}${userPhotoUrl}` : '/assets/images/user-avatar.png';
    }

    constructor(
        public layoutService: LayoutService,
        private http: HttpClient,
        private notificationService: NotificationService,
        private messageService: MessageService,
        private authService: AuthService
    ) {
        this.initializeNotifications();
        this.handleNewNotification();
    }

    ngOnInit(): void {
        this.profileDrawerItems = [
            {

                label: 'Profile',
                icon: 'pi pi-user'

            },
            {
                label: 'Logout',
                icon: 'pi pi-sign-out',
                shortcut: '⌘+Q',
                command: () => {
                    this.logout();
                }
            },
        ];
    }

    public logout(){
        this.authService.logout();
    }

    private initializeNotifications(): void {
        setTimeout(() => {
            if (!this.initialLoaded) {
                this.fetchNotifications();
            }
        }, 5000);
    }

    toggleNotifPanel(event: Event): void {
        if (this.initialLoaded) {
            this.notifPanel.toggle(event);
        } else {
            this.fetchNotifications(event);
        }
    }

    private fetchNotifications(event?: Event): void {
        this.http.get<AppNotificationModel[]>(this.apiUrl, this.httpOptions).subscribe({
            next: (data) => {
                this.notifications = data;
                this.unseenMsgCount = this.notifications.filter(n => !n.isSeen).length;
                this.initialLoaded = true;

                if (event) {
                    this.notifPanel.toggle(event);
                }
            },
            error: (err) => {
                console.error('Error fetching notifications', err);
            }
        });
    }

    private handleNewNotification() {
        this.notificationService.newNotification.subscribe({
            next: (notify: AppNotificationModel) => {
                this.notifications.unshift(notify);
                this.unseenMsgCount += 1;
                this.messageService.add({ key: 'notification', severity: 'info', summary: notify.description });
            }
        });
    }
}
