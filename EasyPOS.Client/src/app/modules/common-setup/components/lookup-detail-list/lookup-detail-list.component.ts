import { AfterViewInit, Component, ElementRef, inject, ViewChild } from '@angular/core';
import { LookupDetailDetailComponent } from '../lookup-detail-detail/lookup-detail-detail.component';
import { AppPageActionModel, LookupDetailsClient, } from 'src/app/modules/generated-clients/api-service';
import { DataGridComponent } from 'src/app/shared/components/data-grid/data-grid.component';
import { ConfirmDialogService } from 'src/app/shared/services/confirm-dialog.service';

@Component({
  selector: 'app-lookup-detail-list',
  templateUrl: './lookup-detail-list.component.html',
  styleUrl: './lookup-detail-list.component.scss',
  providers: [LookupDetailsClient]

})
export class LookupDetailListComponent {
  detailComponent = LookupDetailDetailComponent;
  pageId = '687c6b12-763a-47d7-3f90-08dca9b2d959'

  selectedRows: any;

  @ViewChild('grid') grid: DataGridComponent;

  constructor(public entityClient: LookupDetailsClient) { }


  onHandleToolbarAction(action: AppPageActionModel) {
    if (action.actionName === 'test') {
      this.deleteSelectedItems();
    }
  }

  deleteSelectedItems() {
    const selectedIds = this.selectedRows.map(x => x.id);
    this.grid.confirmDialogService.confirm(`Do you want to delete this?`).subscribe((confirmed) => {
      if (confirmed) {
        this.entityClient.deleteMultiple(selectedIds).subscribe({
          next: () => {
            this.grid.toast.deleted("Multiple Deleted!");
            this.grid.refreshGrid();
          },
          error: (error) => {
            this.grid.toast.showError('Fail to delete.')
          }
        });
      }
    });
  }

}