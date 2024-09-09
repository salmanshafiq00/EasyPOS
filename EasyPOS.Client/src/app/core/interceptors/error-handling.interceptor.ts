import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { Observable, catchError, of, switchMap, throwError } from 'rxjs';

@Injectable()
export class ErrorHandlingInterceptor implements HttpInterceptor {

  constructor(private toastService: MessageService, private router: Router) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        return this.handleError(error);
      })
    );
  }

  private handleError(error: HttpErrorResponse): Observable<any> {
    if (error.error instanceof Blob) {
      return this.blobToText(error.error).pipe(
        switchMap(data => {
          const parsedError = JSON.parse(data);
          return this.processError(parsedError, error);
        })
      );
    } else {
      return this.processError(error.error, error);
    }
  }

  private processError(error: any, httpError: HttpErrorResponse): Observable<any> {
    if (error.status === 403) {
      this.router.navigate(['notfound']);
    } else if (error.status === 404) {
      this.showError(error.errorMessage);
    } else if (error.errorMessage) {
      this.showError(error.errorMessage);
    } else {
      this.showError('An unexpected error occurred.');
    }

    return throwError(() => httpError);
  }

  private blobToText(blob: Blob): Observable<string> {
    return new Observable<string>((observer) => {
      if (!blob) {
        observer.next('');
        observer.complete();
      } else {
        const reader = new FileReader();
        reader.onload = () => {
          observer.next(reader.result as string);
          observer.complete();
        };
        reader.readAsText(blob);
      }
    });
  }

  private showError(message: string) {
    this.toastService.add({
      severity: 'error',
      summary: 'Error',
      detail: message,
      key: 'ErrorHandling',
      life: 3000
    });
  }
}
