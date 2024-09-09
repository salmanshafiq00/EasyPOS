import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpEvent, HttpHandler, HttpRequest, HttpResponse } from '@angular/common/http';
import { Observable, catchError, tap, throwError } from 'rxjs';

@Injectable()
export class LoggingInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    const started = Date.now();
    const correlationId = req.headers.get('X-Correlation-Id');

    // Combine request headers and body into a single JSON object
    const requestLog = {
      headers: Object.fromEntries(req.headers.keys().map(key => [key, req.headers.get(key)])),
      body: this.isFormData(req.body) ? '[FormData]' : req.body
    };

    // Log the combined request object
    console.log(`Requesting to --> ${correlationId} -->  ${req.url}:`, requestLog);

    return next.handle(req).pipe(
      tap((event: HttpEvent<any>) => {
        if (event instanceof HttpResponse) {
          const elapsed = Date.now() - started;

          // Check if the response is a Blob
          if (event.body instanceof Blob) {
            this.readBlobContent(event.body)
            .then((responseData) => {
              console.log(`Response from --> ${this.apiPostfix(req.url)} --> ${correlationId} -> after ${elapsed} ms:`, responseData);
            });
          } else {
            console.log(`Response from --> ${this.apiPostfix(req.url)} --> [${correlationId}] -> after ${elapsed} ms:`, event.body);
          }
        }
      }),
      catchError((error) => {
        const elapsed = Date.now() - started;
        console.error(`Request to ${req.url} failed after ${elapsed} ms:`, error);
        if(error?.error instanceof Blob){
          this.readBlobContent(error.error)
          .then((jsonError) => {
            console.log(`Problem Details: `, jsonError)
          });
        } else {
          console.log(`Problem Details: `, error.error)
        }
        return throwError(() => error);
      })
    );
  }

  private readBlobContent(blob: Blob) {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.onload = () => {
        try {
          const contentType = blob.type;
          if(contentType === 'application/json' || contentType === 'application/problem+json'){
            resolve(JSON.parse(reader.result as string));
          } else {
            resolve(reader.result)
          }

        } catch (error) {
          reject(error)
        }
      };
      reader.onerror = reject;
      reader.readAsText(blob);

    });
  }

  private apiPostfix(url: string): string {
    const apiIndex = url.indexOf('/api/');
    if (apiIndex !== -1) {
      return url.substring(apiIndex + 5)
    } else {
      return ''
    }
  }

  private isFormData(body: any): boolean {
    return body instanceof FormData;
  }

}