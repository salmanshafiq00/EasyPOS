import { Injectable, inject } from "@angular/core";
import { MessageService } from "primeng/api";

@Injectable()
export class ToastService {


  private readonly lifeSpan: number = 3000;
  private readonly key: string = 'toast';
  private messageService: MessageService = inject(MessageService);

  created(title?: string) {
    this.messageService.clear();
    this.messageService.add(
      {
        severity: 'success',
        summary: title || 'Success',
        detail: 'Successfully Created.',
        key: this.key, 
        life: this.lifeSpan
      });
  }

  updated(title?: string) {
    this.messageService.clear();
    this.messageService.add(
      {
        severity: 'success',
        summary: title || 'Success',
        detail: 'Successfully Updated.',
        key: this.key, 
        life: this.lifeSpan
      });
  }

  deleted(title?: string) {
    this.messageService.clear();
    this.messageService.add(
      {
        severity: 'success',
        summary: title || 'Success',
        detail: 'Successfully Deleted.',
        key: this.key, 
        life: this.lifeSpan
      });
  }

  showSuccess(message: string, title?: string) {
    this.messageService.clear();
    this.messageService.add(
      {
        severity: 'success',
        summary: title || 'Success',
        detail: message,
        key: this.key, 
        life: this.lifeSpan
      });
  }

  showError(message: string, title?: string) {
    this.messageService.clear();
    this.messageService.add(
      {
        severity: 'error',
        summary: title || 'Error',
        detail: message,
        key: this.key,
        life: this.lifeSpan
      });
  }

  showInfo(message: string, title?: string) {
    this.messageService.clear();
    this.messageService.add(
      {
        severity: 'info',
        summary: title || 'Info',
        detail: message,
        key: this.key,
        life: this.lifeSpan
      });
  }

  showWarn(message: string, title?: string) {
    this.messageService.clear();
    this.messageService.add(
      {
        severity: 'warn',
        summary: title || 'Warn',
        detail: message,
        key: this.key,
        life: this.lifeSpan
      });
  }

  showContrast(message: string, title?: string) {
    this.messageService.clear();
    this.messageService.add(
      {
        severity: 'contrast',
        summary: title,
        detail: message,
        key: this.key,
        life: this.lifeSpan
      });
  }

  showSecondary(message: string, title?: string) {
    this.messageService.clear();
    this.messageService.add(
      {
        severity: 'secondary',
        summary: title,
        detail: message,
        key: this.key,
        life: this.lifeSpan
      });
  }

  showCustom(message: string, severity?: string, title?: string, key?: string, life?: number) {
    this.messageService.add(
      {
        severity: severity || 'success',
        summary: title,
        detail: message,
        key: key, 
        life: life || this.lifeSpan
      });
  }
}