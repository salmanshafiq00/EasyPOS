import { Component, Inject } from '@angular/core';
import { LookupsClient } from 'src/app/modules/generated-clients/api-service';
import { LookupDetailComponent } from '../lookup-detail/lookup-detail.component';

@Component({
  selector: 'app-lookup-list',
  templateUrl: './lookup-list.component.html',
  styleUrl: './lookup-list.component.scss',
  providers: [LookupsClient]
})
export class LookupListComponent  {
  detailComponent = LookupDetailComponent;
  pageId = '1bea0afd-ebcc-491f-3424-08dca9afe0ca';

  constructor(public entityClient: LookupsClient) {
      
  }
}

