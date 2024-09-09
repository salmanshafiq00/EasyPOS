import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/core/auth/services/auth.service';
import { matchPasswordsValidator } from 'src/app/core/validators/matchPasswords.validator';
import { AppUserModel } from 'src/app/modules/generated-clients/api-service';
import { ToastService } from 'src/app/shared/services/toast.service';

@Component({
  selector: 'user-change-password',
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.scss',
  providers: [AuthService]
})
export class ChangePasswordComponent {

  item: AppUserModel;
  form: FormGroup;

  get f() {
    return this.form.controls;
  }

  private toast: ToastService = inject(ToastService)
  private authService: AuthService = inject(AuthService)
  private fb: FormBuilder = inject(FormBuilder);
  private router: Router = inject(Router);

  ngOnInit(): void {
    this.initializeFormGroup();
  }

  onSubmit() {
    if (this.form.invalid) {
      this.toast.showError('Form is invalid.');
      return;
    }
    this.changePassword();
  }

  private changePassword() {
    const currentPassword = this.form.get('currentPassword').value;
    const newPassword = this.form.get('newPassword').value;
    this.authService.changePassword(currentPassword, newPassword).subscribe(
      {
        next: () => {
          this.authService.clearAccessToken();
          this.toast.updated();
          console.log('updated password')
        }, 
        error: (err) => {
          this.toast.showError('Change password not successful');
        }
      }
    );
  }

  private initializeFormGroup() {
    this.form = this.fb.group({
      currentPassword: [null, Validators.required],
      newPassword: [null, Validators.required],
      confirmPassword: [null, Validators.required]
    }, {
      validators: matchPasswordsValidator('newPassword', 'confirmPassword')
    });
  }
}

