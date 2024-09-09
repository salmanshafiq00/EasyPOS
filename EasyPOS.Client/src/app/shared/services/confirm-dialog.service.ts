import { Injectable, inject } from "@angular/core";
import { ConfirmationService } from "primeng/api";
import { Observable, Subject } from "rxjs";

@Injectable()
export class ConfirmDialogService{

  private confirmService: ConfirmationService = inject(ConfirmationService);

  confirm(message: string, header: string = 'Confirm', icon: string = 'pi pi-question'): Observable<boolean>{
    const confirmSubject = new Subject<boolean>();
    this.confirmService.confirm({
      message: message,
      header: header,
      icon: icon,
      accept: () => {
        confirmSubject.next(true);
        confirmSubject.complete();
      },
      reject: () => {
        confirmSubject.next(false);
        confirmSubject.complete();
      }
    });

    return confirmSubject.asObservable();
  }
}