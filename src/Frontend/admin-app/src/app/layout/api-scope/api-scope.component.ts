import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NzModalRef, NzModalService } from 'ng-zorro-antd/modal';
import { NzNotificationService, NzNotificationPlacement } from 'ng-zorro-antd/notification';
import { NzTableQueryParams } from 'ng-zorro-antd/table';
import { MessageConstants } from '@app/shared/constants/messages.constant';
import { ApiScopeServices } from '@app/shared/services/api-scope.services';
import { ApiScope } from '@app/shared/models/api-scope.model';

@Component({
  selector: 'app-api-scope',
  templateUrl: './api-scope.component.html',
  styleUrls: ['./api-scope.component.scss']
})
export class ApiScopeComponent implements OnInit {
  // Load api data
  public filter = '';
  public pageIndex = 1;
  public pageSize = 10;
  public items: any[];
  public totalRecords: number;
  public name: string;

  // Spin
  public isSpinning: boolean;
  public isSpinningDrawer: boolean;

  // Init form
  validateForm!: FormGroup;

  // Drawer Edit user
  visibleEditApi = false;

  // Modal
  confirmDeleteModal?: NzModalRef;

  constructor(private apiScopeServices: ApiScopeServices,
    private notification: NzNotificationService,
    private modal: NzModalService,
    private fb: FormBuilder) { }

  ngOnInit(): void {
    this.validateForm = this.fb.group({
      enabled: [null],
      name: [null, [Validators.required]],
      displayName: [null, [Validators.required]],
      description: [null],
      required: [null],
      emphasize: [null],
      showInDiscoveryDocument: [null]
    });
    this.loadApiData(this.pageIndex, this.pageSize);
  }

  onQueryParamsChange(params: NzTableQueryParams): void {
    const { pageSize, pageIndex } = params;
    this.loadApiData(pageIndex, pageSize);
  }

  // Load api scopes data
  loadApiData(pageIndex: number, pageSize: number): void {
    this.isSpinning = true;
    this.apiScopeServices.getAllPaging(this.filter, pageIndex, pageSize)
      .subscribe(res => {
        this.items = res.items;
        this.totalRecords = res.totalRecords;
        setTimeout(() => {
          this.isSpinning = false;
        }, 500);
      }, errorMessage => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_ERROR,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          errorMessage,
          'bottomRight'
        );
        setTimeout(() => {
          this.isSpinning = false;
        }, 500);
      });
  }

  // Edit api resource
  openEditApiScope(name: string): void {
    this.visibleEditApi = true;
    this.isSpinningDrawer = true;
    this.apiScopeServices.getDetail(name)
      .subscribe((res: ApiScope) => {
        this.validateForm.setValue({
          enabled: res.enabled,
          name: res.name,
          displayName: res.displayName,
          description: res.description,
          required: res.required,
          emphasize: res.emphasize,
          showInDiscoveryDocument: res.showInDiscoveryDocument
        });
        setTimeout(() => {
          this.isSpinningDrawer = false;
        }, 500);
      }, errorMessage => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_ERROR,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          errorMessage,
          'bottomRight'
        );
        setTimeout(() => {
          this.isSpinningDrawer = false;
        }, 500);
      });
  }

  // Delete api resource
  delete(name: string) {
    this.isSpinning = true;
    this.apiScopeServices.delete(name)
      .subscribe(() => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_DELETE + name + ' !', 'bottomRight');
        this.ngOnInit();
        setTimeout(() => {
          this.isSpinning = false;
        }, 500);
      }, errorMessage => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_ERROR,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          errorMessage,
          'bottomRight'
        );
        setTimeout(() => {
          this.isSpinning = false;
        }, 500);
      });
  }

  showDeleteConfirm(name: string): void {
    this.confirmDeleteModal = this.modal.confirm({
      nzTitle: 'Do you Want to delete api scope ' + name + ' ?',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzOnOk: () =>
        new Promise((resolve, reject) => {
          this.delete(name);
          setTimeout(Math.random() > 0.5 ? resolve : reject, 200);
        })
    });
  }

  closeEditApiScope(): void {
    this.visibleEditApi = false;
  }

  submitForm(): void {
    this.isSpinningDrawer = true;
    const name = this.validateForm.get('name').value;
    this.apiScopeServices.update(name, this.validateForm.getRawValue())
      .subscribe(() => {
        this.visibleEditApi = false;
        this.ngOnInit();
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_UPDATE + name + '!',
          'bottomRight');
      }, errorMessage => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_ERROR,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          errorMessage,
          'bottomRight'
        );
        setTimeout(() => {
          this.isSpinningDrawer = false;
        }, 500);
      });
  }

  // Notification
  createNotification(type: string, title: string, content: string, position: NzNotificationPlacement): void {
    this.notification.create(type, title, content, { nzPlacement: position });
  }
}
