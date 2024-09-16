import { Component, inject, OnInit } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonConstants } from 'src/app/core/contants/common';
import { CommonValidationMessage } from 'src/app/core/contants/forms-validaiton-msg';
import { AppPagesClient, AppPageModel, CreateAppPageCommand, UpdateAppPageCommand } from 'src/app/modules/generated-clients/api-service';
import { CustomDialogService } from 'src/app/shared/services/custom-dialog.service';
import { PrimengIcon } from 'src/app/shared/services/primeng-icon';
import { ToastService } from 'src/app/shared/services/toast.service';

@Component({
  selector: 'app-app-page-detail',
  templateUrl: './app-page-detail.component.html',
  styleUrl: './app-page-detail.component.scss',
  providers: [ToastService, AppPagesClient]
})
export class AppPageDetailComponent implements OnInit {
  VMsg = CommonValidationMessage;
  comConst = CommonConstants;
  optionsDataSources = {};
  form: FormGroup;
  id: string = '';
  activeView: 'design' | 'json' = 'design';
  jsonError: string | null = null;
  item: AppPageModel = new AppPageModel();
  pageLayout: any = {
    'toolbarActions': [],
    'showRowActionCol': true,
    'showSelectCheckbox': false,
    'rowActionType': 'button',
    'gridFilterType': 'basic-top',
    'rowActions': [],
    'appPageFields': [],
  };
  pageLayoutJson: string = '';

  primengIcons = PrimengIcon.primeIcons;

  get f() {
    return this.form?.controls;
  }

  public customDialogService: CustomDialogService = inject(CustomDialogService);
  private toast: ToastService = inject(ToastService);
  private fb: FormBuilder = inject(FormBuilder);

  private entityClient: AppPagesClient = inject(AppPagesClient);

  ngOnInit() {
    this.id = this.customDialogService.getConfigData();
    this.initializeFormGroup();
    this.getById(this.id);
  }

  cancel() {
    this.customDialogService.close(false);
  }

  onSubmit() {
    if (!this.id || this.id === this.comConst.EmptyGuid) {
      console.log(this.form.value)
      this.save();
    } else {
      this.update();
    }
  }

  private save() {
    const command: CreateAppPageCommand = {
      ...this.form.value,
      appPageLayout: JSON.stringify(this.preparePageLayout())
    };

    this.entityClient.create(command).subscribe({
      next: () => {
        this.toast.created()
        this.customDialogService.close(true);
      },
      error: (error) => {
        this.toast.showError(error.errors[0]?.description)
        console.log(error);
      }
    });
  }

  private update() {
    const command: UpdateAppPageCommand = {
      ...this.form.value,
      appPageLayout: JSON.stringify(this.preparePageLayout())
    };
    this.entityClient.update(command).subscribe({
      next: () => {
        this.toast.updated()
        this.customDialogService.close(true);
      },
      error: (error) => {
        this.toast.showError(error.errors[0]?.description)
        console.log(error);
      }
    });
  }

  private getById(id: any) {
    this.entityClient.get(id).subscribe({
      next: (res: AppPageModel) => {
        if (id && id !== CommonConstants.EmptyGuid) {
          this.pageLayout = JSON.parse(res.appPageLayout)
          this.item = res;
          this.item.appPageFields = this.pageLayout.appPageFields || [];
          this.item.toolbarActions = this.pageLayout.toolbarActions || [];
          this.item.rowActions = this.pageLayout.rowActions || [];
          this.item.showRowActionCol = this.pageLayout.showRowActionCol;
          this.item.showSelectCheckbox = this.pageLayout.showSelectCheckbox;
          this.item.rowActionType = this.pageLayout.rowActionType;
          this.item.gridFilterType = this.pageLayout.gridFilterType || 'basic-top';
          this.item.appPageFields?.forEach(() => {
            this.addAppPageField();
          });
          this.item.toolbarActions?.forEach(() => {
            this.addAppPageAction();
          });
          this.item.rowActions?.forEach(() => {
            this.addRowAction();
          })
          this.form.patchValue(this.item);
          this.updateJsonFromForm();
        } else {
          this.item.id = this.id;
          this.defaultAppPageAction();
          this.form.patchValue(this.item);
        }
      },
      error: (error) => {
        console.log(error)
        this.toast.showError(error)
      }
    });
  }

  private preparePageLayout() {
    const pageLayout = {
      toolbarActions: this.form.get('toolbarActions').value,
      showRowActionCol: this.form.get('showRowActionCol').value,
      showSelectCheckbox: this.form.get('showSelectCheckbox').value,
      rowActionType: this.form.get('rowActionType').value,
      gridFilterType: this.form.get('gridFilterType').value,
      rowActions: this.form.get('rowActions').value,
      appPageFields: this.form.get('appPageFields').value,
    };
  
    // Reset showProperties and sort by sortOrder
    ['toolbarActions', 'rowActions', 'appPageFields'].forEach((key) => {
      pageLayout[key] = pageLayout[key]
        .map(item => ({ ...item, showProperties: false }))
        .sort((a, b) => a.sortOrder - b.sortOrder);
    });
  
    return pageLayout;
  }

  private initializeFormGroup() {
    this.form = this.fb.group({
      id: [null],
      title: ['', Validators.required],
      subTitle: [''],
      componentName: ['', Validators.required],
      appPageLayout: [''],
      toolbarActions: this.fb.array([]),
      showRowActionCol: [true],
      showSelectCheckbox: [false],
      rowActionType: ['button'],
      gridFilterType: ['basic-top'],
      rowActions: this.fb.array([]),
      appPageFields: this.fb.array([])
    });
  }

  showProperty(field: AbstractControl, show: boolean): void {
    field.get('showProperties')?.setValue(show);
  }

  showPropertyStatus(field: any) {
    return field.get('showProperties')?.value;
  }

  // JSON //

  showSelectedView(viewName: 'design' | 'json') {
    this.activeView = viewName;
    if (this.activeView === 'json') {
      this.updateJsonFromForm();
    }
  }

  onJsonChange() {
    try {
      const jsonData = JSON.parse(this.pageLayoutJson);
      this.jsonError = null;
      this.updateFormFromJson(jsonData);
    } catch (error) {
      this.jsonError = 'Invalid JSON format';
    }
  }

  private updateJsonFromForm() {
    this.pageLayoutJson = JSON.stringify(this.preparePageLayout(), null, 2);
  }

  private updateFormFromJson(jsonData: any) {

    this.toolbarActions.clear();
    this.rowActions.clear();
    this.appPageFields.clear();

    jsonData.toolbarActions?.forEach(() => {
      this.addAppPageAction();
    });
    jsonData.rowActions?.forEach(() => {
      this.addRowAction();
    })
    jsonData.appPageFields?.forEach(() => {
      this.addAppPageField();
    });

    this.form.patchValue({
      toolbarActions: jsonData.toolbarActions || [],
      showRowActionCol: jsonData.showRowActionCol,
      showSelectCheckbox: jsonData.showSelectCheckbox,
      rowActionType: jsonData.rowActionType,
      gridFilterType: jsonData.gridFilterType,
      rowActions: jsonData.rowActions || [],
      appPageFields: jsonData.appPageFields || []
    });
  }

  // App Page Toolbar Actions //

  get toolbarActions(): FormArray {
    return this.form.get('toolbarActions') as FormArray;
  }


  addAppPageAction(): void {
    this.toolbarActions.push(this.createAppPageAction());
  }

  removeAppPageAction(index: number): void {
    this.toolbarActions.removeAt(index);
  }

  private createAppPageAction(): FormGroup {
    const atn_id = 'atn_' + this.newGuid();
    const sortOrder = this.toolbarActions?.length + 1 ?? 1;
    return this.fb.group({
      id: [atn_id],
      actionName: ['', Validators.required],
      actionType: ['button', Validators.required],
      caption: [''],
      icon: [null],
      permissions: [''],
      functionName: [''],
      navigationUrl: [''],
      position: ['left'],
      severity: ['primary'],
      sortOrder: [sortOrder],
      isVisible: [true],
      showCaption: [true],
      params: [''],
      showProperties: [true]
    });
  }

  private defaultAppPageAction() {
    const atn_id_1 = 'atn_' + this.newGuid();
    const atn_id_2 = 'atn_' + this.newGuid();
    const toolbarActions = this.fb.array([
      this.fb.group({
        id: [atn_id_1],
        actionName: ['new', Validators.required],
        actionType: ['button', Validators.required],
        caption: ['New'],
        icon: ['pi pi-plus'],
        permissions: [''],
        navigationUrl: [''],
        position: ['left'],
        severity: ['success'],
        sortOrder: [1],
        isVisible: [true],
        showCaption: [true],
        params: [''],
        showProperties: [false]
      }),
      this.fb.group({
        id: [atn_id_2],
        actionName: ['refresh', Validators.required],
        actionType: ['button', Validators.required],
        caption: ['Refresh'],
        icon: ['pi pi-sync'],
        permissions: [''],
        navigationUrl: [''],
        position: ['left'],
        severity: ['help'],
        sortOrder: [2],
        isVisible: [true],
        showCaption: [true],
        params: [''],
        showProperties: [false]
      })
    ]);

    this.form.setControl('toolbarActions', toolbarActions);
  }

  // Row Action //

  get rowActions(): FormArray {
    return this.form.get('rowActions') as FormArray;
  }

  addRowAction(): void {
    this.rowActions.push(this.createRowAction());
  }

  removeRowAction(index: number): void {
    this.rowActions.removeAt(index);
  }

  private createRowAction(): FormGroup {
    const ract_id = 'ract_' + this.newGuid();
    const sortOrder = this.rowActions?.length + 1 ?? 1;
    return this.fb.group({
      id: [ract_id],
      actionName: ['', Validators.required],
      caption: [''],
      icon: [null],
      permissions: [''],
      navigationUrl: [''],
      params: [''],
      severity: ['primary'],
      sortOrder: [sortOrder],
      isVisible: [true],
      showCaption: [true],
      showProperties: [true]
    });
  }

  // App Page Fields //

  get appPageFields(): FormArray {
    return this.form.get('appPageFields') as FormArray;
  }

  addAppPageField(): void {
    this.appPageFields.push(this.createAppPageField());
  }

  removeAppPageField(index: number): void {
    this.appPageFields.removeAt(index);
  }

  createAppPageField(): FormGroup {
    const fld_id = 'fld_' + this.newGuid();
    const sortOrder = this.appPageFields?.length + 1 ?? 1;
    return this.fb.group({
      id: [fld_id],
      field: ['', Validators.required],
      header: [''],
      fieldType: ['string'],
      dbField: [''],
      format: [''],
      textAlign: ['center'],
      isSortable: [true],
      isFilterable: [false],
      isGlobalFilterable: [false],
      filterType: [null],
      dsName: [''],
      enableLink: [false],
      linkBaseUrl: [''],
      linkValueFieldName: [''],
      bgColor: [''],
      color: [''],
      isVisible: [true],
      isActive: [true],
      sortOrder: [sortOrder],
      showProperties: [true],
    });
  }

  private newGuid() {
    return 'xxx4x'.replace(/[xy]/g, function (c) {
      const r = Math.random() * 16 | 0,
        v = c === 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }

  fieldTypeSelectList = [
    { 'id': 'string', 'name': 'String' },
    { 'id': 'date', 'name': 'Date' },
    { 'id': 'datetime', 'name': 'Date Time' },
    { 'id': 'daterange', 'name': 'Date Range' },
    { 'id': 'time', 'name': 'Time' },
    { 'id': 'number', 'name': 'Number' },
    { 'id': 'image', 'name': 'Image' }
  ];

  alignSelectList = [
    { 'id': 'left', 'name': 'Left' },
    { 'id': 'center', 'name': 'Center' },
    { 'id': 'right', 'name': 'Right' }
  ];

  filterTypeSelectList = [
    { 'id': 'string', 'name': 'String' },
    { 'id': 'select', 'name': 'Select' },
    { 'id': 'multiselect', 'name': 'Multi-Select' },
    { 'id': 'date', 'name': 'Date' },
    { 'id': 'datetime', 'name': 'Date Time' },
    { 'id': 'daterange', 'name': 'Date Range' },
    { 'id': 'time', 'name': 'Time' },
  ];

  actionButtonsPositionSelectList = [
    { 'id': 'left', 'name': 'Left' },
    { 'id': 'right', 'name': 'Right' },
  ];

  gridFilterTypeSelectList = [
    { 'id': 'basic', 'name': 'Basic' },
    { 'id': 'basic-top', 'name': 'Basic-Top' },
    { 'id': 'advanced', 'name': 'Advanced' }
  ];

  actionTypeSelectList = [
    { 'id': 'button', 'name': 'Button' },
    { 'id': 'multi-select', 'name': 'Multi-Select' },
    { 'id': 'dropdown', 'name': 'Dropdown' },
    { 'id': 'upload', 'name': 'Upload' },
    { 'id': 'routerLink', 'name': 'RouterLink' }
  ];

  severitySelectList = [
    { 'id': 'primary', 'name': 'Primary' },
    { 'id': 'secondary', 'name': 'Decondary' },
    { 'id': 'success', 'name': 'Success' },
    { 'id': 'info', 'name': 'Info' },
    { 'id': 'warning', 'name': 'Warning' },
    { 'id': 'help', 'name': 'Help' },
    { 'id': 'danger', 'name': 'Danger' },
    { 'id': 'contrast', 'name': 'Contrast' }
  ];

  // Drag & Drop //

  draggedItem: any;

  dragStart(event: DragEvent, index: number, formArray: FormArray) {
    this.draggedItem = formArray.at(index);
  }

  dragEnd(event: DragEvent) {
    this.draggedItem = null;
  }

  drop(event: DragEvent, index: number, formArray: FormArray) {
    if (this.draggedItem) {
      const draggedIndex = this.findFormArrayIndex(formArray, this.draggedItem);
      this.moveItemInFormArray(formArray, draggedIndex, index);
      this.updateSortOrder(formArray); // for sortorder start from 1 instead of 0
      this.draggedItem = null;
    }
  }
  findFormArrayIndex(formArray: FormArray<any>, item: AbstractControl): number {
    return formArray.controls.indexOf(item);
  }

  moveItemInFormArray(formArray: FormArray, fromIndex: number, toIndex: number) {
    if (fromIndex === toIndex) return;

    const item = formArray.at(fromIndex);
    formArray.removeAt(fromIndex);
    formArray.insert(toIndex, item);
  }

  updateSortOrder(formArray: FormArray) {
    formArray.controls.forEach((control, index) => {
      control.get('sortOrder').setValue(index + 1);
    });
  }

}
