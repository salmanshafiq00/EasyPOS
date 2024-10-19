import { Component, inject } from '@angular/core';
import { CompanyInfoDetailComponent } from '../company-info-detail/company-info-detail.component';
import { CompanyInfosClient } from 'src/app/modules/generated-clients/api-service';

@Component({
  selector: 'app-company-info-list',
  templateUrl: './company-info-list.component.html',
  styleUrl: './company-info-list.component.scss',
  providers: [CompanyInfosClient]
})
export class CompanyInfoListComponent {
  detailComponent = CompanyInfoDetailComponent;
  pageId = ''

  entityClient: CompanyInfosClient = inject(CompanyInfosClient);
}
