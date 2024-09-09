import { Injectable, Type, inject } from "@angular/core";
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from "primeng/dynamicdialog";
import { BehaviorSubject, Observable, Subject } from "rxjs";

@Injectable()
export class CustomDialogService {

  private defaultConfig: DynamicDialogConfig = {
    width: '50vw',
    resizable: true,
    draggable: true,
    modal: true,
    breakpoints: {
      '960px': '75vw',
      '640px': '90vw'
    }
  };

  private ref: DynamicDialogRef | undefined;
  private dialogService: DialogService = inject(DialogService);
  private dataSubject = new BehaviorSubject<any>(null);

  open<T>(
    component: Type<any>,
    data: T,
    header: string,
    config?: Partial<DynamicDialogConfig>
  ): Observable<boolean> {

    const mergedConfig = { ...this.defaultConfig, ...config, data: data, header: header };

    this.ref = this.dialogService.open(component, mergedConfig);

    this.dataSubject.next(data);

    // const onClose$ = new Subject<boolean>();
    // this.ref.onClose.subscribe((result: boolean) => {
    //   onClose$.next(result);
    //   onClose$.complete();
    // });

    // return onClose$.asObservable();
    if (this.ref) {
      return this.ref.onClose as Observable<boolean>;
    } else {
      console.error('Dialog reference is undefined.');
      return new Observable<boolean>(); // return an empty observable if ref is undefined
    }
  }

  getConfigData<T>(): T {
    return this.dataSubject.value as T;
  }

  close(succeeded: boolean) {
    if(this.ref){
      this.ref.close(succeeded);
    }
    else {
      console.error('Dialog reference is undefined.');
    }
  }
  
}