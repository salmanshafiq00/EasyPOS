import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AppUserModel, UsersClient } from 'src/app/modules/generated-clients/api-service';
import { ToastService } from 'src/app/shared/services/toast.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'user-user-basic',
  templateUrl: './user-basic.component.html',
  styleUrl: './user-basic.component.scss',
  providers: [UsersClient]
})
export class UserBasicComponent {

  item: AppUserModel;
  form: FormGroup;
  photoUrl: string;
  isEditing: boolean = false;

  get f() {
    return this.form.controls;
  }

  constructor(
    private entityClient: UsersClient,
    private toast: ToastService,
    private fb: FormBuilder) {
  }

  ngOnInit(): void {
    this.initializeFormGroup();
    this.getProfile();
  }

  onSubmit() {
    if (this.form.invalid) {
      this.toast.showError('Form is invalid.');
      return;
    }
    this.updateBasic();
  }

  onEditProfile() {
    this.isEditing = true;
  }

  onCancel() {
    this.isEditing = false;
  }

  private updateBasic() {
    this.entityClient.updateBasic({ ...this.form.value }).subscribe({
      next: () => {
        this.item = {...this.item, ...this.form.value};
        console.log(this.item)
        this.isEditing = false;
        this.toast.updated();
      },
      error: (error) => {
        this.toast.showError(error[0]?.description);
      }
    });
  }

  private getProfile() {
    this.entityClient.getProfile().subscribe({
      next: (res: any) => {
        this.item = res;
        this.photoUrl = environment.API_BASE_URL + this.item.photoUrl 
        this.form.patchValue({
          ...this.item
        });
      },
      error: (error) => {
        this.toast.showError(error[0]?.description);
      }
    });
  }

  private initializeFormGroup() {
    this.form = this.fb.group({
      id: [''],
      firstName: ['', Validators.required],
      lastName: [''],
      username: ['', Validators.required],
      email: ['', Validators.required],
      phoneNumber: [''],
    });

  }
}
