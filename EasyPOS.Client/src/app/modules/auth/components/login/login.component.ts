import { Component, inject } from '@angular/core';
import { LayoutService } from 'src/app/layout/service/app.layout.service';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/core/auth/services/auth.service';
import { LoginRequestCommand } from 'src/app/core/auth/services/auth-client.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styles: [`
  :host ::ng-deep .pi-eye,
  :host ::ng-deep .pi-eye-slash {
      transform:scale(1.6);
      margin-right: 1rem;
      color: var(--primary-color) !important;
  }
`],
  providers: [AuthService]
})
export class LoginComponent {

  email: string = 'administrator@localhost';
  password: string = 'Salman@123';
  isRemember: boolean = false;

  public layoutService: LayoutService = inject(LayoutService);
  private authService: AuthService = inject(AuthService)
  private router: Router = inject(Router)

  login(){
    const loginRequest = new LoginRequestCommand({userName: this.email, password: this.password, isRemember: this.isRemember})
    this.authService.login(loginRequest).subscribe({
      next: (isSuccessful) => {
        this.router.navigate(['/']);
      },
      error: (err) => {
        alert('Login Error!!!')
      }
    });

  }
}
