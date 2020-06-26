import { Component, OnInit, OnDestroy } from '@angular/core';
import { NzTableQueryParams } from 'ng-zorro-antd/table';
import { IdentityResourceServices } from '@app/shared/services/identity-resources.service';
import { NzNotificationPlacement, NzNotificationService } from 'ng-zorro-antd/notification';
import { catchError, map } from 'rxjs/operators';
import { MessageConstants } from '@app/shared/constants/messages.constant';
import { throwError } from 'rxjs';
import { NzModalRef, NzModalService } from 'ng-zorro-antd/modal';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IdentityResource } from '@app/shared/models/identity-resource.model';

@Component({
  selector: 'app-identity-resource',
  templateUrl: './identity-resource.component.html',
  styleUrls: ['./identity-resource.component.scss']
})
export class IdentityResourceComponent implements OnInit {

  // Load identity data
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
  visibleEditIdentity = false;

  // Modal
  confirmDeleteModal?: NzModalRef;

  constructor(private identityResourceServices: IdentityResourceServices,
    private notification: NzNotificationService,
    private modal: NzModalService,
    private fb: FormBuilder) { }

  ngOnInit(): void {
    this.validateForm = this.fb.group({
      name: [null, [Validators.required]],
      displayName: [null, Validators.required],
      description: [null],
      enabled: [true],
      showInDiscoveryDocument: [true],
      required: [false],
      emphasize: [false]
    });
    this.loadIdentityData(this.pageIndex, this.pageSize);
  }

  onQueryParamsChange(params: NzTableQueryParams): void {
    const { pageSize, pageIndex } = params;
    this.loadIdentityData(pageIndex, pageSize);
  }

  // Load identity data
  loadIdentityData(pageIndex: number, pageSize: number): void {
    this.isSpinning = true;
    this.identityResourceServices.getAllPaging(this.filter, pageIndex, pageSize)
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

  // Edit identity resource
  openEditIdentityResource(name: string): void {
    this.visibleEditIdentity = true;
    this.isSpinningDrawer = true;
    this.identityResourceServices.getDetail(name)
      .subscribe((res: IdentityResource) => {
        this.validateForm.setValue({
          name: res.name,
          displayName: res.displayName,
          description: res.description,
          enabled: res.enabled,
          showInDiscoveryDocument: res.showInDiscoveryDocument,
          required: res.required,
          emphasize: res.emphasize
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

  // Delete identity resource
  delete(name: string) {
    if (name === 'openid' || name === 'profile') {
      this.createNotification(
        MessageConstants.TYPE_NOTIFICATION_ERROR,
        MessageConstants.TITLE_NOTIFICATION_SSO,
        'Cannot delete Identity resource openid or profile!',
        'bottomRight'
      );
    } else {
      this.isSpinning = true;
      this.identityResourceServices.delete(name)
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
  }

  showDeleteConfirm(name: string): void {
    this.confirmDeleteModal = this.modal.confirm({
      nzTitle: 'Do you Want to delete identity resource ' + name + ' ?',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzOnOk: () =>
        new Promise((resolve, reject) => {
          this.delete(name);
          setTimeout(Math.random() > 0.5 ? resolve : reject, 200);
        })
    });
  }

  closeEditIdentityResource(): void {
    this.visibleEditIdentity = false;
  }

  submitForm(): void {
    this.isSpinningDrawer = true;
    const name = this.validateForm.get('name').value;
    this.identityResourceServices.update(name, this.validateForm.getRawValue())
      .subscribe(() => {
        this.visibleEditIdentity = false;
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
