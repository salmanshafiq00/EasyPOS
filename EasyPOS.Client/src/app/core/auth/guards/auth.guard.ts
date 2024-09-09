import { Router, RouterStateSnapshot } from "@angular/router";
import { AuthService } from "../services/auth.service";
import { inject } from "@angular/core";

export const AuthGuard = (state: RouterStateSnapshot) => {
  const authService: AuthService = inject(AuthService);
  const router: Router = inject(Router);

  if(authService.isAuthenticated){
    return true;
  }
  else {
    authService.redirectUrl = state.url;
    router.navigate(['/auth/login']);
    return false;
  }
}

export const childAuthGuard = (state: RouterStateSnapshot) => {
  return AuthGuard(state);
}