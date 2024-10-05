import { Component, inject } from '@angular/core';
import { UnitDetailComponent } from '../unit-detail/unit-detail.component';
import { UnitsClient } from 'src/app/modules/generated-clients/api-service';

@Component({
  selector: 'app-unit-list',
  templateUrl: './unit-list.component.html',
  styleUrl: './unit-list.component.scss',
  providers: [UnitsClient]
})
export class UnitListComponent {
  detailComponent = UnitDetailComponent;
  pageId = '1824a984-dff7-467f-28a0-08dce51cb777'

  entityClient: UnitsClient = inject(UnitsClient);
}
