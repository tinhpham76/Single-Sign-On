import { Component, OnInit, OnDestroy } from '@angular/core';
import { RolesServices } from '@app/shared/services/role.service';
import { throwError } from 'rxjs';
import { Role } from '@app/shared/models/role.model';
import { catchError } from 'rxjs/operators';
import { NzTableQueryParams } from 'ng-zorro-antd/table';
import { NzNotificationService, NzNotificationPlacement } from 'ng-zorro-antd/notification';
import { NzModalService, NzModalRef } from 'ng-zorro-antd/modal';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MessageConstants } from '@app/shared/constants/messages.constant';


@Component({
  selector: 'app-role',
  templateUrl: './role.component.html',
  styleUrls: ['./role.component.scss']
})
export class RoleComponent implements OnInit {

  // load role data
  public filter = '';
  public pageIndex = 1;
  public pageSize = 10;
  public items: any[];
  public totalRecords: number;

  // button confirmModal
  public confirmModal?: NzModalRef;

  // load role detail
  public id: string;
  public name: string;
  public normalizedName: string;

  // show edit role
  public showEditRole = false;

  // show Add role
  public visibleAddRole = false;

  // Form
  public formEditRole!: FormGroup;
  public formAddRole!: FormGroup;

  // Spin
  public isSpinning: boolean;

  constructor(
    private rolesService: RolesServices,
    private notification: NzNotificationService,
    private modal: NzModalService,
    private fb: FormBuilder) { }

  // Load dữ liệu và khai báo form
  ngOnInit(): void {
    // Khởi tạo form add
    this.formAddRole = this.fb.group({
      id: [null, [Validators.required]],
      name: [null, [Validators.required]]
    });
    // Khởi tạo form edit
    this.formEditRole = this.fb.group({
      id: [null, [Validators.required]],
      name: [null, [Validators.required]]
    });
    this.loadRoleData(this.filter, this.pageIndex, this.pageSize);
  }

  // Load Role
  loadRoleData(filter: string, pageIndex: number, pageSize: number): void {
    this.isSpinning = true;
    this.rolesService.getAllPaging(filter, pageIndex, pageSize)
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

  // Thêm mới role
  openAddRole(): void {
    this.visibleAddRole = true;
  }

  closeAddRole(): void {
    this.visibleAddRole = false;
  }

  createRole(): void {
    const id = this.formAddRole.get('id').value;
    const name = this.formAddRole.get('name').value;
    const data = this.formAddRole.getRawValue();
    if (id === name) {
      this.rolesService.add(data)
        .subscribe(() => {
          this.createNotification(
            MessageConstants.TYPE_NOTIFICATION_SUCCESS,
            MessageConstants.TITLE_NOTIFICATION_SSO,
            MessageConstants.NOTIFICATION_ADD + id + '!',
            'bottomRight');
          setTimeout(() => {
            this.closeAddRole();
            this.ngOnInit();
          }, 500);
        }, errorMessage => {
          this.createNotification(
            MessageConstants.TYPE_NOTIFICATION_ERROR,
            MessageConstants.TITLE_NOTIFICATION_SSO,
            errorMessage,
            'bottomRight'
          );
          setTimeout(() => {
          }, 500);
        });
    } else {
      this.createNotification(
        MessageConstants.TYPE_NOTIFICATION_ERROR,
        MessageConstants.TITLE_NOTIFICATION_SSO,
        'Role id and Role name values ​​must be equal!',
        'bottomRight');
    }
  }

  // Update Role
  showModal(roleId: string): void {
    this.showEditRole = true;
    this.rolesService.getDetail(roleId)
      .subscribe((res: Role) => {
        this.formEditRole.setValue({
          id: res.id,
          name: res.name
        });
      }, errorMessage => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_ERROR,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          errorMessage,
          'bottomRight'
        );
        setTimeout(() => {
        }, 500);
      });
  }

  handleCancel(): void {
    this.showEditRole = false;
  }

  saveChanges() {
    const id = this.formEditRole.get('id').value;
    const name = this.formEditRole.get('name').value;
    if (id === name) {
      this.rolesService.update(id, this.formEditRole.getRawValue())
        .subscribe(() => {
          this.createNotification(
            MessageConstants.TYPE_NOTIFICATION_SUCCESS,
            MessageConstants.TITLE_NOTIFICATION_SSO,
            MessageConstants.NOTIFICATION_UPDATE + id + '!',
            'bottomRight');
          this.ngOnInit();
          setTimeout(() => {
            this.showEditRole = false;
          }, 500);
        }, errorMessage => {
          this.createNotification(
            MessageConstants.TYPE_NOTIFICATION_ERROR,
            MessageConstants.TITLE_NOTIFICATION_SSO,
            errorMessage,
            'bottomRight'
          );
          setTimeout(() => {
          }, 500);
        });
    } else {
      this.createNotification(
        MessageConstants.TYPE_NOTIFICATION_ERROR,
        MessageConstants.TITLE_NOTIFICATION_SSO,
        'Role id and Role name values ​​must be equal!',
        'bottomRight');
    }
  }

  // Delete Role
  delete(roleId: string) {
    if (roleId === 'Admin') {
      this.createNotification(
        MessageConstants.TYPE_NOTIFICATION_ERROR,
        MessageConstants.TITLE_NOTIFICATION_SSO,
        'Role Admin cannot delete!',
        'bottomRight'
      );
    } else {
      this.isSpinning = true;
      this.rolesService.delete(roleId)
        .subscribe(() => {
          setTimeout(() => {
            this.isSpinning = false;
            this.ngOnInit();
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
            this.ngOnInit();
          }, 500);
        });
    }

  }

  showConfirm(roleId: string): void {
    this.confirmModal = this.modal.confirm({
      nzTitle: 'Do you Want to delete ' + roleId + ' role ?',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzOnOk: () =>
        new Promise((resolve, reject) => {
          this.delete(roleId);
          setTimeout(Math.random() > 0.5 ? resolve : reject, 200);
        })
    });
  }

  // Hiển thị thêm dữ liệu khác
  onQueryParamsChange(params: NzTableQueryParams): void {
    const { pageSize, pageIndex } = params;
    this.loadRoleData(this.filter, pageIndex, pageSize);
  }

  // Tạo thông báo
  createNotification(type: string, title: string, content: string, position: NzNotificationPlacement): void {
    this.notification.create(type, title, content, { nzPlacement: position });
  }

}
