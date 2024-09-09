import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CheckboxModule } from 'primeng/checkbox';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { environment } from 'src/environments/environment';
import { API_BASE_URL } from '../generated-clients/api-service';
import { AuthRoutingComponents, AuthRoutingModule } from './auth-routing.module';
import { ButtonModule } from 'primeng/button';




@NgModule({
  declarations: [
    ...AuthRoutingComponents
  ],
  imports: [
    CommonModule,
    RouterModule,
    ButtonModule,
    CheckboxModule,
    InputTextModule,
    FormsModule,
    PasswordModule,
    AuthRoutingModule
  ],
  providers: [
    { provide: API_BASE_URL, useValue: environment.API_BASE_URL },
  ]
})
export class AuthModule { }
