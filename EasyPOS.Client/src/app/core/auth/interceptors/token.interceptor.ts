import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";
import { AuthService } from "../services/auth.service";
import { EMPTY, Observable, throwError } from "rxjs";
import { Router } from "@angular/router";

@Injectable()
export class TokenInterceptor implements HttpInterceptor{
  
  authService: AuthService = inject(AuthService);
  router: Router = inject(Router);

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    // Check if the request is for authentication (login, refresh token, etc.)
    if (this.isAuthRequest(req.url)) {
      return next.handle(req); // Skip interception for auth requests
    }

    const accessToken = this.authService.getAccessToken();
    
    if(accessToken){
      const clonedRequest = req.clone({
        setHeaders: {
          Authorization: `Bearer ${accessToken}`,
        }
      });
      return next.handle(clonedRequest);
    } else {
      this.router.navigate(['/auth/login']);
      // return EMPTY;
      return throwError(() => new Error('No access token available, redirecting to login.'));

    }
  }

  private isAuthRequest(url: string): boolean {
    // Adjust this logic based on your API routes for authentication
    return url.includes('/Accounts/Login');
  }
}