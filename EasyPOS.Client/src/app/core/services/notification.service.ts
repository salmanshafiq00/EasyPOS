import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr'
import { Subject } from 'rxjs';
import { AppNotificationModel } from 'src/app/modules/generated-clients/api-service';
import { environment } from 'src/environments/environment';
import { AuthService } from '../auth/services/auth.service';

@Injectable()
export class NotificationService {
  private hubConnection: signalR.HubConnection;
  public newNotification: Subject<AppNotificationModel> = new Subject<AppNotificationModel>();
  public permissionChanged: Subject<boolean> = new Subject<boolean>();

  constructor(private authService: AuthService) {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.API_BASE_URL}/notificationHub`, {
        accessTokenFactory: () => this.authService.getAccessToken(),
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      })
      .build();

    this.hubConnection.on('ReceiveNotification', (notification: AppNotificationModel) => {
      if(notification.recieverId?.toLowerCase() === this.authService.getUserId()?.toLowerCase()){
        this.newNotification.next(notification);
      }
    });

    this.hubConnection.on('ReceiveRolePermissionNotify', () => {
        this.permissionChanged.next(true);
    });

    this.startConnection();
  }

  private startConnection() {
    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch((error) => console.log('Error while starting connection: ', error));
  }

  public sendNotification(userId: string, message: any) {
    this.hubConnection.invoke('SendNotification', userId, message)
      .catch(err => console.log(err));
  }
}
