import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { MenuItem } from 'primeng/api';

@Injectable({
  providedIn: 'root'
})
export class BreadcrumbService {
  private breadcrumbSubject = new BehaviorSubject<MenuItem[]>([]);
  breadcrumbs$ = this.breadcrumbSubject.asObservable();

  setBreadcrumbs(breadcrumbs: MenuItem[]) {
    this.breadcrumbSubject.next(breadcrumbs);
  }
}
