import { Component } from '@angular/core';
import { AppMenuDetailComponent } from '../app-menu-detail/app-menu-detail.component';
import { AppMenusClient } from 'src/app/modules/generated-clients/api-service';

@Component({
  selector: 'app-app-menu-list',
  templateUrl: './app-menu-list.component.html',
  styleUrl: './app-menu-list.component.scss',
  providers: [AppMenusClient]

})
export class AppMenuListComponent {
  detailComponent = AppMenuDetailComponent;
  pageId = '5255d7a0-49b8-45da-3f93-08dca9b2d959';

  constructor(public entityClient: AppMenusClient){
    
  }
}

