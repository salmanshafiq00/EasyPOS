import { Component, ElementRef, EventEmitter, Input, OnDestroy, OnInit, Output, ViewChild, inject } from '@angular/core';
import { FilterMatchMode, FilterMetadata, FilterService } from 'primeng/api';
import { Table, TableLazyLoadEvent } from 'primeng/table';
import { timer } from 'rxjs';
import { FieldType, FilterType } from 'src/app/core/contants/FieldDataType';
import { AppPageActionModel, AppPageFieldModel, AppPageModel, AppPagesClient, DataFilterModel, GlobalFilterFieldModel } from 'src/app/modules/generated-clients/api-service';
import { BackoffService } from '../../services/backoff.service';
import { ToastService } from '../../services/toast.service';
import { DatePipe } from '@angular/common';
import { ConfirmDialogService } from '../../services/confirm-dialog.service';
import { CustomDialogService } from '../../services/custom-dialog.service';
import { AppDataGridModel } from '../../models/app-data-grid.model';
import { AppPageDetailComponent } from 'src/app/modules/admin/components/app-page-detail/app-page-detail.component';
import { PermissionService } from 'src/app/core/auth/services/permission.service';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';


@Component({
  selector: 'app-data-grid',
  templateUrl: './data-grid.component.html',
  styleUrl: './data-grid.component.scss',
  providers: [BackoffService, ConfirmDialogService, DatePipe, AppPagesClient]
})
export class DataGridComponent implements OnInit, OnDestroy {
 baseUrl = environment.API_BASE_URL;

  // Page Layout Settings Start Start
  isPagelayoutFound: boolean = true;
  appPageLayout: any = {};
  // appPageToolbarActions: AppPageActionModel[] = [];
  leftToolbarActions: AppPageActionModel[] = [];
  rightToolbarActions: AppPageActionModel[] = [];
  rowActions: AppPageActionModel[] = [];
  dataFields: AppPageFieldModel[] = [];
  appPageModel: AppPageModel = null;
  globalfiltersTooltip: string = '';


  // Page Layout Settings Start End

  FieldType = FieldType;
  FilterType = FilterType;
  FilterMatchModes = FilterMatchMode;
  emptyGuid = '00000000-0000-0000-0000-000000000000';
  // Table Settings //
  isInitialLoaded: boolean = false;
  responsiveLayout = 'scroll';
  cols: any[] = [];
  filters: DataFilterModel[] = [];
  lazyLoading: boolean = true;
  selectionMode: "single" | "multiple" = "multiple";
  rowHover: boolean = true;
  loading: boolean = false;
  dataKey: string = 'id';

  // Pagination
  paginator: boolean = true;
  rowsPerPageOptions: number[] = [5, 10, 20, 30, 50]
  showCurrentPageReport: boolean = true;
  totalRecords: number = 0;
  totalPages: number = 0;
  pageNumber: number = 1;
  first: number = 0;
  rows: number = 10;
  currentPageReportTemplate: string = `Showing {first} to {last} of ${this.totalRecords} entries`;

  // Filtering Global
  globalFilterFields: string[] = [];
  globalFilterFieldModels: GlobalFilterFieldModel[] = [];
  filterType: 'basic' | 'basic-top' | 'advanced' = 'basic-top';
  // globalFilterFieldNames: string[] = [];

  // Global filters
  @ViewChild('dt') table: Table;
  @ViewChild('globalSearchInput') globalSearchInput: ElementRef;
  debounceTimeout: number = 500;


  optionsDataSources = {};

  items: any[] = [];
  _selectedRows: any[] = [];

  // Dropdwon Selected value
  selectedParent: any;
  selectedStatus: any;
  // Centralized storage for dynamic dropdown values
  dynamicDropdownValues: { [key: string]: any } = {};

  // 
  @Input() pageId: string;
  @Input() entityClient: any;
  @Input() detailComponent: any;
  @Input() dialogSize: any = '900px';
  @Input() getFuncName = 'getAll';
  @Input() pageTitle: string = null;
  @Input() listComponent: any;
  @Input() dialogTitle: string = 'Entity Detail';
  @Input() resizableColumns: boolean = true;
  @Input() columnResizeMode: 'fit' | 'expand' = 'expand';
  @Input() styleClass: string = 'p-datatable-sm'; // 'p-datatable-sm p-datatable-gridlines'
  // @Input() dataTableSize: string = 'p-datatable-sm';

  @Output() handleToolbarAction: EventEmitter<AppPageActionModel> = new EventEmitter<AppPageActionModel>();
  @Output() handleGridRowAction: EventEmitter<{ action: AppPageActionModel, data: any }> = new EventEmitter<{ action: AppPageActionModel, data: any }>();

  @Input() get selectedRows(): any[] {
    return this._selectedRows;
  }
  set selectedRows(value: any[]) {
    this._selectedRows = value;
    this.selectedRowsChange.emit(this._selectedRows)
  }

  @Output() selectedRowsChange = new EventEmitter<any[]>();

  get hasSelectOrDateType(): boolean {
    return this.dataFields.some(col => col.fieldType === FilterType.select || col.fieldType === FilterType.multiSelect);
  }


  private backoffService = inject(BackoffService);
  public toast = inject(ToastService);
  public confirmDialogService = inject(ConfirmDialogService);
  public datePipe = inject(DatePipe);
  public customDialogService = inject(CustomDialogService);
  private appPagesClient = inject(AppPagesClient);
  public router: Router = inject(Router);
  public permit = inject(PermissionService);

  ngOnInit() {
    this.loadGridLayout();
    this.loadData({ first: this.first, rows: this.rows }, true)
  }

  ngOnDestroy() {
    // this.searchSubject.unsubscribe();
  }

  loadGridLayout() {
    if (this.pageId) {
      this.appPagesClient.get(this.pageId).subscribe({
        next: (data: AppPageModel) => {
          if (data) {            
            this.appPageModel = data;

            this.appPageLayout = data.appPageLayout ? JSON.parse(data.appPageLayout) : null;
            
            this.filterType = this.appPageLayout?.gridFilterType || this.filterType;
            
            this.pageTitle = this.pageTitle ?? this.appPageModel?.title ?? this.listComponent.constructor.name;
            
            this.dataFields = this.appPageLayout?.appPageFields?.filter(field => field.isVisible === true) || [];
            
            this.leftToolbarActions = this.appPageLayout?.toolbarActions?.filter(action => action.position === 'left' && action.isVisible === true) || [];
            
            this.rightToolbarActions = this.appPageLayout?.toolbarActions?.filter(action => action.position === 'right' && action.isVisible === true) || [];
            
            this.rowActions = this.appPageLayout?.rowActions?.filter(field => field.isVisible === true) || [];
            
            this.globalFilterFields = this.appPageLayout?.appPageFields?.filter(x => x.isGlobalFilterable)?.map(x => x.field) || [];
            
            this.appPageLayout?.appPageFields?.filter(x => x.isGlobalFilterable)?.forEach(field => {
              this.globalFilterFieldModels.push(new GlobalFilterFieldModel({
                field: field.field,
                fieldType: field.fieldType,
                dbField: field.dbField
              }));
            });

            this.globalfiltersTooltip = this.appPageLayout?.appPageFields
              .filter(field => field.isVisible && field.isGlobalFilterable)?.map(x => x.header)
              .join(', ') ?? '';

            this.createDataFilterModelList();
          } else {
            this.isPagelayoutFound = false;
            // TODO: createNewpageLayout()
          }
        },
        error: (error) => {
          console.log(error)
        }
      });
    }
  }


  loadData(event: TableLazyLoadEvent, allowCache?: boolean) {
    this.loading = true;
    this.first = event.first;
    this.rows = event.rows;

    const query = new AppDataGridModel();
    query.offset = this.first;
    query.pageSize = this.rows;
    query.allowCache = !!allowCache;
    query.sortField = this.getSortedField(event);
    query.sortOrder = event.sortOrder;
    query.globalFilterValue = this.getGlobalFilterValue(event);
    query.globalFilterFields = this.globalFilterFieldModels;
    query.isInitialLoaded = this.isInitialLoaded;

    if (this.filterType === 'advanced') {
      this.mapAndSetToDataFilterModel(event.filters);
    } else {
      this.mapAndSetToDataFilterModelForBasic(event.filters);
    }

    query.filters = this.filters;


    if (query.sortField
      || query.globalFilterValue
      || query.filters.length !== 0) {
      query.allowCache = false;
    }

    // console.log(query)
    this.entityClient?.[this.getFuncName](query)?.subscribe({
      next: (res) => {
        this.items = res.items;
        this.pageNumber = res.pageNumber;
        this.totalRecords = res.totalCount;
        this.totalPages = res.totalPages;
        this.currentPageReportTemplate = `Showing {first} to {last} of ${this.totalRecords} entries`;
        // option datasource only assign initially not second time
        if (!this.isInitialLoaded) {
          this.optionsDataSources = res.optionsDataSources;
          this.isInitialLoaded = true;
        }
      },
      error: (error) => {
        console.error(error, 'Error while fetching data')
      },
      complete: () => {
        this.loading = false;
      }
    });
  }

  private createDataFilterModelList() {
    this.dataFields.filter(field => field.isFilterable === true).forEach(field => {
      this.filters.push(new DataFilterModel({
        field: field.field,
        fieldType: field.fieldType,
        filterType: field.filterType,
        dsName: field.dsName,
        dbField: field.dbField
      }));
    });
  }


  // TODO: Here this.loadData() function called twice instead of once
  // refreshGrid() {
  //   this.loading = true;

  //   const delay = this.backoffService.getDelay();
  //   console.log(`Applying delay: ${delay}ms`);
  //   timer(delay).subscribe(() => {
  //     this.clear();
  //     this.loadData({ first: this.first, rows: this.rows }, false);
  //     // this.backoffService.resetDelay();  // need more analyze
  //   });

  // }
  // Function to generate unique keys for dropdowns

  refreshGrid() {
    this.loadData({ first: this.first, rows: this.rows }, false);
  }

  generateDropdownKey(field: string, type: 'select' | 'multiSelect'): string {
    return `${field}_${type}`;
  }


  onSearchInput(event: Event): void {
    const searchText = (event.target as HTMLInputElement).value;
    this.onGlobalFilter(searchText);
  }

  onSearchEnter(event: any): void {
    if (event.key === 'Enter') {
      const searchText = (event.target as HTMLInputElement).value;
      this.table.filterDelay = 0;
      this.onGlobalFilter(searchText);
    }
  }

  // handleAction(action: AppPageActionModel) {
  handleToolbarActions(action: AppPageActionModel) {
    if (action.actionType === 'routerLink') {
      this.router.navigate([action.navigationUrl])
    } else if (action.actionName === 'new') {
      this.openDialog(this.emptyGuid)
    } else if (action.actionName === 'refresh') {
      this.refreshGrid();
    } else if (action.actionName === 'multiple-delete') {
      this.deleteSelectedItems();
    } else if (action.actionName === 'csv') {
      this.table.exportCSV();
    } else if (action.actionName === 'pdf') {
      this.exportPdf();
    } else if (action.actionType === 'routerLink') {
      this.router.navigate([action.navigationUrl])
    }
    // else if(action.actionType === 'multi-select') {
    //   this.handleToolbarAction.emit(action)
    // } 
    else {
      this.handleToolbarAction.emit(action)
    }
  }

  // onRowSelect(event: any) {
  //   this.selectedRows.emit(this.selectedItems);
  // }

  // onRowUnselect(event: any) {
  //   this.selectedRows.emit(this.selectedItems)
  // };

  handleRowAction(action: AppPageActionModel, item: any) {
    if (action.actionName === 'edit') {
      this.edit(item)
    } else if (action.actionName === 'delete') {
      this.delete(item)
    } else {
      this.handleGridRowAction.emit({ action: action, data: item })
    }
  }

  handleRowRouterLinkAction(routerLink: string, item: any, params: string = '') {
    let paramsName = [];
    if (params) {
      paramsName = params?.split(',');
    }
    if (paramsName && paramsName.length > 0) {
      paramsName.forEach(param => {
        const paramValue = item[param.trim()];
        routerLink += `/${paramValue}`;
      });
    }
    this.router.navigate([routerLink])
  }

  private onGlobalFilter(searchText: string, matchMode: string = 'contains'): void {
    this.table.filterGlobal(searchText, matchMode);
    this.table.filterDelay = this.debounceTimeout;
  }

  clear() {
    this.table.clear();
    this.globalSearchInput.nativeElement.value = '';
  }


  private getGlobalFilterValue(event: TableLazyLoadEvent): string {
    if (typeof event.globalFilter === 'string') {
      // If event.globalFilter is already a string, directly assign it to query.globalFilterValue
      return event.globalFilter.trim();
    } else if (Array.isArray(event.globalFilter)) {
      // If event.globalFilter is an array of strings, join the array elements into a single string
      return event.globalFilter.join(', '); // or any other delimiter you prefer
    }

    // Default return value if event.globalFilter is neither string nor array
    return '';
  }

  private getSortedField(event: TableLazyLoadEvent): string {
    if (typeof event.sortField === 'string') {
      return event.sortField;
    } else if (Array.isArray(event.sortField)) {
      return event.sortField.join(', '); // or any other delimiter you prefer
    }
    return '';
  }

  private mapAndSetToDataFilterModel(filters: FilterDictionary) {
    for (const field in filters) {
      if (field === 'global') continue;

      // to ensure that properties are not being accessed from the prototype chain unintentionally
      if (Object.prototype.hasOwnProperty.call(filters, field)) {

        // existingFilter is exist
        const existingFilter = this.filters.find(x => x.field === field);

        if (!existingFilter) continue;

        const filterMetadata = filters[field];

        if (Array.isArray(filterMetadata)) {
          let isFirstFilterMetaData = true;
          for (const filter of filterMetadata) {

            if (existingFilter && existingFilter.filterType == FilterType.date) {
              if (isFirstFilterMetaData) {
                existingFilter.value = this.getTranformValue(existingFilter, filter);
                existingFilter.matchMode = filter.matchMode || '';
                existingFilter.operator = filter.operator || '';
                isFirstFilterMetaData = false;
              } else {
                const newFilter = new DataFilterModel();
                newFilter.field = field;
                newFilter.fieldType = existingFilter.fieldType;
                newFilter.filterType = existingFilter.filterType;
                newFilter.value = this.getTranformValue(existingFilter, filter);
                newFilter.matchMode = filter.matchMode || '';
                newFilter.operator = filter.operator || '';
                newFilter.dsName = existingFilter.dsName;
                this.filters.push(newFilter);
              }
            }
            else if (existingFilter) {
              existingFilter.value = this.getTranformValue(existingFilter, filter);
              existingFilter.matchMode = filter.matchMode || '';
              existingFilter.operator = filter.operator || '';
            }
          }
        } else if (typeof filterMetadata === 'object' && filterMetadata !== null && !Array.isArray(filterMetadata)) {
          if (existingFilter) {
            existingFilter.value = this.getTranformValue(filterMetadata.value, filterMetadata);
            existingFilter.matchMode = filterMetadata.matchMode || '';
            existingFilter.operator = filterMetadata.operator || '';
          }
        }
      }
    }
  }

  private mapAndSetToDataFilterModelForBasic(filters: FilterDictionary) {
    for (const field in filters) {
      if (field === 'global') continue;

      // to ensure that properties are not being accessed from the prototype chain unintentionally
      if (filters.hasOwnProperty(field)) {

        // existingFilter is exist
        const existingFilter = this.filters.find(x => x.field === field);

        if (!existingFilter) continue;

        const filterMetadata = filters?.[field];

        if (typeof filterMetadata === 'object' && filterMetadata !== null && !Array.isArray(filterMetadata)) {
          existingFilter.value = this.getTranformValue(existingFilter, filterMetadata);
          existingFilter.matchMode = filterMetadata.matchMode || '';
          existingFilter.operator = filterMetadata.operator || '';
        }
      }
    }
  }

  private getTranformValue(filter: DataFilterModel, filterMetadata: FilterMetadata): string {
    if (filter.fieldType !== FieldType.number && Array.isArray(filterMetadata.value)) {
      return filterMetadata.value.map(item => `'${item.id}'`).join(', ');
    } 
    else if (filter.fieldType === FieldType.number && Array.isArray(filterMetadata.value)) {
      return filterMetadata.value.map(item => `${item.id}`).join(', ');
    }
    else if (filter.fieldType !== FieldType.number && filter.filterType == FilterType.select) {
      return filterMetadata.value !== null ? `'${filterMetadata.value}'` : '';
    } 
    else if (filter.fieldType === FieldType.number && filter.filterType == FilterType.select) {
      return filterMetadata.value !== null ? filterMetadata.value.toString() : '';
    }
    else if (filter.filterType == FilterType.string) {
      return filterMetadata.value !== null ? filterMetadata.value.toString() : '';
    }
    else if (filter.filterType == FilterType.date) {
      return filterMetadata.value ? this.datePipe.transform(filterMetadata.value, 'yyyy/MM/dd') : '';
    }
    else {
      return '';
    }
  }

  ///  -------------  Edit & Delete -------------------

  edit(item: any) {
    this.openDialog(item.id);
  }

  delete(item: any) {
    this.confirmDialogService.confirm(`Do you want to delete this?`).subscribe((confirmed) => {
      if (confirmed) {
        this.deleteItem(item.id);
      }
    });
  }

  private deleteItem(id: string) {
    this.entityClient.delete(id).subscribe({
      next: () => {
        this.toast.deleted();
        this.loadData({ first: this.first, rows: this.rows }, false)
      },
      error: (error) => {
        this.toast.showError('Fail to delete.')
      }
    });
  }


  deleteSelectedItems() {
    const selectedIds = this.selectedRows.map(x => x.id);
    this.confirmDialogService.confirm(`Do you want to delete this?`).subscribe((confirmed) => {
      if (confirmed) {
        this.entityClient.deleteMultiple(selectedIds).subscribe({
          next: () => {
            this.toast.deleted("Multiple Deleted!");
            this.loadData({ first: this.first, rows: this.rows }, false)
          },
          error: (error) => {
            this.toast.showError('Fail to delete.')
          }
        });
      }
    });
  }


  ///  -------------  Dialog -------------------

  openDialog(data: any) {
    this.customDialogService.open<string>(
      this.detailComponent,
      data,
      this.dialogTitle
    )
      .subscribe((isSucceed: boolean) => {
        if (isSucceed) {
          this.loadData({ first: this.first, rows: this.rows })
        }
      });
  }


  // -------------- Page Setting -----------------
  showPageSetting() {
    if (this.pageId && this.pageId !== this.emptyGuid) {
      this.openPageSettingDialog(this.pageId)
    }
  }

  openPageSettingDialog(pageId: string) {
    this.customDialogService.open<string>(
      AppPageDetailComponent,
      pageId,
      'Page Setting'
    )
      .subscribe((isSucceed: boolean) => {
        if (isSucceed) {
          this.loadGridLayout();
          this.clear();
        }
      });
  }

  // --------- Export to PDF ---------------------

  exportPdf() {
    const visibleFields = this.dataFields.filter(col => col.isVisible);
    const headers = visibleFields.map(col => col.header);
    const fields = visibleFields.map(col => col.field);

    const data = this.items.map(row => fields.map(field => row[field]));

    const today = this.datePipe.transform(new Date(), 'dd/MM/yyyy');

    // Generate filter text
    const filterTextLines = this.filters
      .filter(filter => filter.value)
      .map(filter => {
        const dataField = this.dataFields.find(x => x.field === filter.field);
        let filterValue = filter.value;
        
        if (dataField.fieldType === 'select' || dataField.fieldType === 'multiselect') {
          const dataSource = this.optionsDataSources[dataField?.dsName]?.find(
            x => x.id?.toString().toLowerCase() == filterValue.toLowerCase().replace(/^'|'$/g, ''));
          filterValue = dataSource?.name;
        }

        console.log(dataField)
        return `${dataField.header}: ${filterValue}`
    });

    import("jspdf").then(jsPDF => {
      import("jspdf-autotable").then(autoTable => {
        const doc = new jsPDF.default();

        // Page Title
        doc.setFontSize(16);
        const pageWidth = doc.internal.pageSize.getWidth();
        const textWidth = doc.getTextWidth(this.pageTitle);
        const textX = (pageWidth - textWidth) / 2;
        doc.text(this.pageTitle, textX, 10);

        // Add Filters
        let startY = 30; // Starting Y position for filters
        const tableStartX = 10; // This should match the table's left margin
        doc.setFontSize(12); // Adjust the font size for filters

        // Split filters into two columns
        const maxFiltersPerLine = Math.ceil(filterTextLines.length / 2);
        const leftColumnFilters = filterTextLines.slice(0, maxFiltersPerLine);
        const rightColumnFilters = filterTextLines.slice(maxFiltersPerLine);

        leftColumnFilters.forEach((leftLine, index) => {
          const rightLine = rightColumnFilters[index] || '';
          doc.text(leftLine, tableStartX, startY); // Align with table's left side
          doc.text(rightLine, pageWidth / 2 + tableStartX, startY); // Align right-side filters with a gap from the middle of the page
          startY += 10; // Move to the next line
        });

        // Page Grid
        autoTable.default(doc, {
          startY: startY, // Start the table below the filters
          head: [headers],
          body: data,
          margin: { left: tableStartX }, // Align table with filters
          didDrawPage: function (data) {
            // Printed Date
            doc.setFontSize(8);
            const dateStr = 'printed date: ' + today;
            const pageHeight = doc.internal.pageSize.getHeight();
            const marginX = pageWidth - doc.getTextWidth(dateStr) - 15; // Position from the right edge
            const marginY = pageHeight - 10; // Position from the bottom edge
            doc.text(dateStr, marginX, marginY);
          }
        });

        doc.save(`${this.pageTitle}.pdf`);
      });
    });
  }




}

export interface FilterDictionary {
  [s: string]: FilterMetadata | FilterMetadata[];
}