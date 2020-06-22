import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NzModalRef, NzModalService } from 'ng-zorro-antd/modal';
import { ApiResourceServices } from '@app/shared/services/api-resources.service';
import { NzNotificationService, NzNotificationPlacement } from 'ng-zorro-antd/notification';
import { NzTableQueryParams } from 'ng-zorro-antd/table';
import { catchError } from 'rxjs/operators';
import { MessageConstants } from '@app/shared/constants/messages.constant';
import { throwError } from 'rxjs';
import { ApiResource } from '@app/shared/models/api-resource.model';

@Component({
  selector: 'app-api-resource',
  templateUrl: './api-resource.component.html',
  styleUrls: ['./api-resource.component.scss']
})
export class ApiResourceComponent implements OnInit {

  // Load api data
  public filter = '';
  public pageIndex = 1;
  public pageSize = 5;
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

  constructor(private apiResourceServices: ApiResourceServices,
    private notification: NzNotificationService,
    private modal: NzModalService,
    private fb: FormBuilder) { }

  ngOnInit(): void {
    this.validateForm = this.fb.group({
      name: [null, [Validators.required]],
      displayName: [null, Validators.required],
      description: [null],
      enabled: [true],
    });
    this.loadApiData(this.pageIndex, this.pageSize);
  }

  onQueryParamsChange(params: NzTableQueryParams): void {
    const { pageSize, pageIndex } = params;
    this.loadApiData(pageIndex, pageSize);
  }

  // Load api data
  loadApiData(pageIndex: number, pageSize: number): void {
    this.isSpinning = true;
    this.apiResourceServices.getAllPaging(this.filter, pageIndex, pageSize)
      .pipe(catchError(err => {
        this.isSpinning = true;
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_ERROR,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_ERROR,
          'bottomRight'
        );
        return throwError('Error', err);
      }))
      .subscribe(res => {
        this.items = res.items;
        this.totalRecords = res.totalRecords;
        setTimeout(() => {
          this.isSpinning = false;
        }, 500);
      });
  }

  // Edit api resource
  openEditApiResource(name: string): void {
    this.visibleEditApi = true;
    this.isSpinningDrawer = true;
    this.apiResourceServices.getDetail(name)
      .pipe(catchError(err => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_ERROR,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_ERROR,
          'bottomRight'
        );
        return throwError('Error', err);
      }))
      .subscribe((res: ApiResource) => {
        this.validateForm.setValue({
          name: res.name,
          displayName: res.displayName,
          description: res.description,
          enabled: res.enabled,
        });
        setTimeout(() => {
          this.isSpinningDrawer = false;
        }, 500);
      });
  }

  // Delete api resource
  delete(name: string) {
    this.isSpinning = true;
    this.apiResourceServices.delete(name)
      .pipe(catchError(err => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_ERROR,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_ERROR,
          'bottomRight'
        );
        return throwError('Error', err);
      }))
      .subscribe(res => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_DELETE + name + ' !', 'bottomRight');
        this.ngOnInit();
        setTimeout(() => {
          this.isSpinning = false;
        }, 500);
      });
  }

  showDeleteConfirm(name: string): void {
    this.confirmDeleteModal = this.modal.confirm({
      nzTitle: 'Do you Want to delete api resource ' + name + ' ?',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzOnOk: () =>
        new Promise((resolve, reject) => {
          this.delete(name);
          setTimeout(Math.random() > 0.5 ? resolve : reject, 200);
        })
    });
  }

  closeEditApiResource(): void {
    this.visibleEditApi = false;
  }

  submitForm(): void {
    this.isSpinningDrawer = true;
    const name = this.validateForm.get('name').value;
    this.apiResourceServices.update(name, this.validateForm.getRawValue())
      .pipe(catchError(err => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_ERROR,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_ERROR,
          'bottomRight'
        );
        return throwError('Error', err);
      }))
      .subscribe(() => {
        this.visibleEditApi = false;
        this.ngOnInit();
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_UPDATE + name + '!',
          'bottomRight');
      });
  }

  // Notification
  createNotification(type: string, title: string, content: string, position: NzNotificationPlacement): void {
    this.notification.create(type, title, content, { nzPlacement: position });
  }
}