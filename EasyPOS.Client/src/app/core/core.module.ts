import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { TokenInterceptor } from './auth/interceptors/token.interceptor';
import { RefreshTokenInterceptor } from './auth/interceptors/refresh-token.interceptor';
import { AuthService } from './auth/services/auth.service';
import { API_BASE_URL, AccountsClient } from './auth/services/auth-client.service';
import { environment } from 'src/environments/environment';
import { CorrelationIdInterceptor } from './interceptors/correlation-id.interceptor';
import { LoggingInterceptor } from './interceptors/logging.interceptor';
import { ErrorHandlingInterceptor } from './interceptors/error-handling.interceptor';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { NotificationService } from './services/notification.service';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    ToastModule
  ],
  providers: [
    AuthService,
    AccountsClient,
    MessageService,
    NotificationService,
    {provide: API_BASE_URL, useValue: environment.API_BASE_URL},
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorHandlingInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: RefreshTokenInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: CorrelationIdInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: LoggingInterceptor, multi: true },
  ],
  exports: [
    ToastModule,
  ]
})
export class CoreModule { }
