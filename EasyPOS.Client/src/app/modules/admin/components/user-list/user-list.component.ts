import { Component } from '@angular/core';
import { UsersClient } from 'src/app/modules/generated-clients/api-service';
import { UserDetailComponent } from '../user-detail/user-detail.component';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.scss',
  providers: [UsersClient]
})
export class UserListComponent {
  detailComponent = UserDetailComponent;
  pageId = '4007d345-6ede-43f3-3f91-08dca9b2d959';
  
  constructor(public entityClient: UsersClient) {

  }

}
