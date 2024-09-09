import { Injectable, inject } from '@angular/core';
import { Observable, catchError, map, of, tap } from 'rxjs';
import { AccountsClient, AuthenticatedResponse, ChangePasswordCommand, IChangePasswordCommand, LoginRequestCommand } from './auth-client.service';
import { Router } from '@angular/router';
import { jwtDecode } from "jwt-decode";


const Access_Token = 'Access_Token';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  // private _authenticated: boolean = false;
  redirectUrl: string | null = null;

  private accountsClient: AccountsClient = inject(AccountsClient);
  private router: Router = inject(Router);

  get isAuthenticated(): boolean{
    return !!this.getAccessToken();
  }

  login(loginRequest: LoginRequestCommand): Observable<boolean> {
    return this.accountsClient.login(loginRequest).pipe(
      map((response) => {
        if(!response){
          return false;
        }
        else{
          this.setAccessToken(response.accessToken);
          return true;
        }
      }),
      catchError((err) => {
        console.error(`error while login`, err);
        return of(false);
      })
    );
  }

  logout() {
    this.accountsClient.logout().subscribe({
      next: () => {
        this.clearAccessToken();
        this.router.navigate(['/auth/login'])
      }, 
      error: (err) => {
        this.clearAccessToken();
        this.router.navigate(['/auth/login'])
      }
    });
  }

  changePassword(currentPassword: string, newPassword: string): Observable<void> {
    const command = new ChangePasswordCommand({
      currentPassword: currentPassword,
      newPassword: newPassword,
    });

    return this.accountsClient.changePassword(command);
  }

  refreshToken(): Observable<AuthenticatedResponse> {
    return this.accountsClient.refreshToken()
      .pipe(
        map((authResponse) => {
          localStorage.setItem(Access_Token, `${authResponse.accessToken}`);
          return authResponse;
        }),
        catchError((error) => {
          console.error(`Error refresh token`, error);
          return of(error);
        }));
  }


  decodeToken(): JwtPayload{
    try{
      return jwtDecode<JwtPayload>(this.getAccessToken());
    } catch (error){
      console.log('Invalid Token', error)
      return null;
    }
  }

  getUserRoles(): string[]{
    const decodedToken = this.decodeToken();
    return decodedToken ? decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] : [];
  }

  getUserId(): string{
    const decodedToken = this.decodeToken();
    return decodedToken ? decodedToken.sub : '';
  }

  getPhotoUrl(): string{
    const decodedToken = this.decodeToken();
    return decodedToken ? decodedToken.photoUrl : '';
  }

  getAccessToken(): string | null {
    return localStorage.getItem(Access_Token);
  }

  setAccessToken(value: string) {
    localStorage.setItem(Access_Token, value);
  }

  clearAccessToken(): void {
    localStorage.removeItem(Access_Token);
  }
}

export interface JwtPayload {
  sub: string;  // Subject (User ID)
  email: string;  // User's email
  jti: string;  // JWT ID
  username: string;  // Username
  photoUrl: string;  // PhotoUrl
  ip: string;  // IP address
  "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": string[];  // Array of roles
  exp: number;  // Expiration time (Unix timestamp)
  iss: string;  // Issuer
  aud: string;  // Audience
}

