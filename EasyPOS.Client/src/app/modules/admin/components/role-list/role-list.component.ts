import { Component} from '@angular/core';
import { RolesClient } from 'src/app/modules/generated-clients/api-service';
import { RoleDetailComponent } from '../role-detail/role-detail.component';

@Component({
  selector: 'app-role-list',
  templateUrl: './role-list.component.html',
  styleUrl: './role-list.component.scss',
  providers: [RolesClient]

})
export class RoleListComponent {
  detailComponent = RoleDetailComponent;
  pageId = '9a08e9a6-c641-44b1-3f92-08dca9b2d959';

  constructor(public entityClient: RolesClient) {
      
  }

}