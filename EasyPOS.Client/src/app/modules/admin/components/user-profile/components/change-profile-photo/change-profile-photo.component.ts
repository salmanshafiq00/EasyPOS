import { HttpHeaders, HttpEvent, HttpEventType, HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { Observable, map } from 'rxjs';
import { ChangeUserPhotoCommand, UsersClient } from 'src/app/modules/generated-clients/api-service';

@Component({
  selector: 'user-change-profile-photo',
  templateUrl: './change-profile-photo.component.html',
  styleUrl: './change-profile-photo.component.scss',
  providers: [UsersClient]
})
export class ChangeProfilePhotoComponent {
  
  uploadedFileUrls: string[] = [];

  private entityService = inject(UsersClient);
  private toast = inject(UsersClient);

  onFileUpload(fileUrls: string[]){
    console.log(fileUrls)
    const command = new ChangeUserPhotoCommand({ photoUrl: fileUrls[0] });

    this.entityService.changePhoto(command).subscribe({
      next: () => {
        localStorage.setItem('userPhotoUrl', fileUrls[0])
      }
    });

  }

}
