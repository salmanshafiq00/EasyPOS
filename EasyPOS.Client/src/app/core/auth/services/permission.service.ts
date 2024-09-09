import { inject, Injectable } from '@angular/core';
import { AccountsClient } from './auth-client.service';

@Injectable({
  providedIn: 'root'
})
export class PermissionService {

  private permissions: Set<string> = new Set();

  private accountsClient: AccountsClient = inject(AccountsClient);

  loadPermissions(allowCache: boolean = true){
    this.accountsClient.getUserPermissions(allowCache).subscribe({
      next: (permits: string[]) => {
        this.permissions = new Set<string>(permits);
      },
      error: (error) => {
        console.error('Failed to load permissions:', error);
      }
    });
  }

  hasPermission(permission: string = ''): boolean {
    if(permission.trim() === '') return true;
    return this.permissions.has(permission.trim());
  }

}
