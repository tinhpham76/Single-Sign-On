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
export class RoleComponent implements OnInit, OnDestroy {

  // load role data
  filter = '';
  pageIndex = 1;
  pageSize = 5;
  items: any[];
  totalRecords: number;

  // button confirmModal
  confirmModal?: NzModalRef;

  // load role detail
  id: string;
  name: string;
  normalizedName: string;

  // show edit role
  showEditRole = false;

  // show Add role
  visibleAddRole = false;

  // Form
  formEditRole!: FormGroup;
  formAddRole!: FormGroup;

  // Spin
  isSpinning = true;

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
    this.rolesService.getAllPaging(filter, pageIndex, pageSize)
    .pipe(catchError(err => {
      this.createNotification(
        MessageConstants.TYPE_NOTIFICATION_ERROR,
        MessageConstants.TITLE_NOTIFICATION_SSO,
        MessageConstants.NOTIFICATION_ERROR,
        'bottomRight'
      );
      return throwError('Error');
    }))
      .subscribe(res => {
        this.items = res.items;
        this.totalRecords = res.totalRecords;
        setTimeout(() => {
          this.isSpinning = false;
        }, 500);
      }, error => {
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
    const data = this.formAddRole.getRawValue();
    this.rolesService.add(data)
      .pipe(catchError(err => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_ERROR,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_ERROR,
          'bottomRight'
        );
        return throwError('Error');
      }))
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
      },
        error => {
          setTimeout(() => {
          }, 500);
        });
  }

  // Update Role

  showModal(roleId: string): void {
    this.showEditRole = true;
    this.rolesService.getDetail(roleId)
    .pipe(catchError(err => {
      this.createNotification(
        MessageConstants.TYPE_NOTIFICATION_ERROR,
        MessageConstants.TITLE_NOTIFICATION_SSO,
        MessageConstants.NOTIFICATION_ERROR,
        'bottomRight'
      );
      return throwError('Error');
    }))
      .subscribe((res: Role) => {
        this.formEditRole.setValue({
          id: res.id,
          name: res.name
        });
      });
  }

  handleCancel(): void {
    this.showEditRole = false;
  }

  saveChanges() {
    const id = this.formEditRole.get('id').value;
    this.rolesService.update(id, this.formEditRole.getRawValue())
      .pipe(catchError(err => {
        this.createNotification(
          MessageConstants.NOTIFICATION_ERROR,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_ERROR,
          'bottomRight'
        );
        return throwError('Error');
      }))
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
      }, error => {
        setTimeout(() => {
          this.showEditRole = false;
        }, 500);
      });
  }

  // Delete Role

  delete(roleId) {
    this.rolesService.delete(roleId)
      .pipe(catchError(err => {
        this.createNotification(
          MessageConstants.NOTIFICATION_ERROR,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_ERROR,
          'bottomRight'
        );
        return throwError('Error');
      }))
      .subscribe(res => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_DELETE + roleId + ' !', 'bottomRight');
        this.ngOnInit();
      });
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

  //

  ngOnDestroy(): void { }
}
