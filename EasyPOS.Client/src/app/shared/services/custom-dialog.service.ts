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

  private dialogRefs: DynamicDialogRef[] = [];  // Keep track of all open dialogs
  private ref: DynamicDialogRef | undefined;
  private dialogService: DialogService = inject(DialogService);
  private dataSubject = new BehaviorSubject<any>(null);
  public closeDataSubject = new Subject<any>();
  public handelCloseIconClick = new Subject<boolean>();
  // public isLastClosedSuccessfully: boolean = false;
  public handleCloseIcon: boolean = false;

  open<T>(
    component: Type<any>,
    data: T,
    header: string,
    config?: Partial<DynamicDialogConfig>
  ): Observable<boolean> {

    const mergedConfig = { ...this.defaultConfig, ...config, data: data, header: header };

    this.ref = this.dialogService.open(component, mergedConfig);

    this.dataSubject.next(data);

    if (this.ref) {
      return this.ref.onClose as Observable<boolean>;
    } else {
      console.error('Dialog reference is undefined.');
      return new Observable<boolean>(); // return an empty observable if ref is undefined
    }
  }

  /**
   * Gets the current config data stored in the BehaviorSubject.
   */
  getConfigData<T>(): T {
    return this.dataSubject.value as T;
  }

  closeWithData<T>(succeeded: boolean, data: T) {
    if(this.ref){
      this.ref.close(succeeded);
      this.closeDataSubject.next(data);
    }
    else {
      console.error('Dialog reference is undefined.');
    }
  }

  close(succeeded: boolean) {
    if(this.ref){
      this.ref.close(succeeded);
      if(succeeded){
        this.ref.destroy();
      }
    }
    else {
      console.error('Dialog reference is undefined.');
    }
  }

  /**
   * Opens a new dialog and adds the reference to the dialog stack.
   */
  openDialog<T>(
    component: Type<any>, 
    data: T, 
    header: string, 
    config?: Partial<DynamicDialogConfig>, 
    handleCloseIcon: boolean = false): DynamicDialogRef {
    // this.isLastClosedSuccessfully = false;
    this.handleCloseIcon = handleCloseIcon;
    const mergedConfig = { ...this.defaultConfig, ...config, data: data, header: header };
    const ref = this.dialogService.open(component, mergedConfig);

    // Push the dialog ref to the stack
    this.dialogRefs.push(ref);
    this.dataSubject.next(data);

    ref.onClose.subscribe((result) => {
      // if(result != undefined){
      //   this.isLastClosedSuccessfully = result;
      // }
      if(this.handleCloseIcon){
        this.handelCloseIconClick.next(handleCloseIcon);
        this.handleCloseIcon = false;
      }
    });

    return ref;  // Return the reference for the caller to manage
  }

  /**
   * Close only the most recently opened dialog (the top of the stack).
   */
  closeLastDialog(succeeded: boolean) {
    if (this.dialogRefs.length > 0) {
      const ref = this.dialogRefs.pop();  // Get the most recent dialog reference
      if (ref) {
        ref.close(succeeded);
        ref.destroy();
      }
    } else {
      console.error('No dialog reference found to close.');
    }
  }

    /**
   * Close only the most recently opened dialog (the top of the stack).
   */
    closeLastDialogWithData<T>(succeeded: boolean, data: T) {
      if (this.dialogRefs.length > 0) {
        const ref = this.dialogRefs.pop();  // Get the most recent dialog reference
        if (ref) {
          ref.close(succeeded);
          this.closeDataSubject.next(data);
          ref.destroy();
        }
      } else {
        console.error('No dialog reference found to close.');
      }
    }

  /**
   * Closes all dialogs currently open.
   */
  closeAllDialogs(succeeded: boolean) {
    while (this.dialogRefs.length > 0) {
      const ref = this.dialogRefs.pop();
      if (ref) {
        ref.close(succeeded);
        ref.destroy();
      }
    }
  }

   /**
   * Close a specific dialog by reference.
   */
   closeDialog(ref: DynamicDialogRef, succeeded: boolean) {
    const index = this.dialogRefs.indexOf(ref);
    if (index !== -1) {
      this.dialogRefs.splice(index, 1);  // Remove dialog ref from the stack
      ref.close(succeeded);
      ref.destroy();
    } else {
      console.error('Dialog reference not found in the stack.');
    }
  }

  
}