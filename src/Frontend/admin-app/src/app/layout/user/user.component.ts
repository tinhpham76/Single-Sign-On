import { Component, OnInit } from '@angular/core';
import { UserServices } from '@app/shared/services/users.services';
import { MessageConstants } from '@app/shared/constants/messages.constant';
import { NzNotificationService, NzNotificationPlacement } from 'ng-zorro-antd/notification';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { NzTableQueryParams } from 'ng-zorro-antd/table';
import { NzModalRef, NzModalService } from 'ng-zorro-antd/modal';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { User } from '@app/shared/models/user.model';
import { HttpClient } from '@angular/common/http';
import { DatePipe } from '@angular/common';
import { Role } from '@app/shared/models/role.model';
import { RolesServices } from '@app/shared/services/role.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss'],
  providers: [DatePipe]
})
export class UserComponent implements OnInit {

  // Load user data
  filter = '';
  pageIndex = 1;
  pageSize = 5;
  items: any[];
  totalRecords: number;

  // Spin
  isSpinning = true;

  // Confirm delete user
  confirmDeleteModal?: NzModalRef;
  confirmResetModal?: NzModalRef;

  // Drawer Edit user
  visibleEditUser = false;
  formEditUser!: FormGroup;

  // Edit user role
  showEditUserRole = false;

  constructor(private userServices: UserServices,
    private notification: NzNotificationService,
    private modal: NzModalService,
    private fb: FormBuilder,
    private datePipe: DatePipe,
    private roleServices: RolesServices) { }

  ngOnInit(): void {
    this.formEditUser = this.fb.group({
      id: [null],
      userName: [null],
      firstName: [null, [Validators.required]],
      lastName: [null, [Validators.required]],
      email: [null, [Validators.email, Validators.required]],
      phoneNumberPrefix: ['+84'],
      phoneNumber: [null, [Validators.required]],
      dob: [null, [Validators.required]]
    });
    this.loadUserData(this.filter, this.pageIndex, this.pageSize);
  }

  // Load user data

  loadUserData(filter: string, pageIndex: number, pageSize: number): void {
    this.userServices.getAllPaging(this.filter, pageIndex, pageSize)
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

  onQueryParamsChange(params: NzTableQueryParams): void {
    const { pageSize, pageIndex } = params;
    this.loadUserData(this.filter, pageIndex, pageSize);
  }


  // Get Detail & Edit User

  openEditUser(userId: string): void {
    this.visibleEditUser = true;
    this.userServices.getDetail(userId)
      .pipe(catchError(err => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_ERROR,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_ERROR,
          'bottomRight'
        );
        return throwError('Error');
      }))
      .subscribe((res: User) => {
        const dob = this.datePipe.transform(res.dob, 'yyy/MM/dd');
        this.formEditUser.setValue({
          id: res.id,
          userName: res.userName,
          firstName: res.firstName,
          lastName: res.lastName,
          email: res.email,
          phoneNumberPrefix: '+84',
          phoneNumber: res.phoneNumber,
          dob: dob
        });
      });
  }

  closeEditUser(): void {
    this.visibleEditUser = false;
  }

  editUser(): void {
    const id = this.formEditUser.get('id').value;
    this.userServices.update(id, this.formEditUser.getRawValue())
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
          this.visibleEditUser = false;
        }, 500);
      }, error => {
        setTimeout(() => {
          this.visibleEditUser = false;
        }, 500);
      });
  }

  saveChanges() {
    const userId = this.formEditUser.get('id').value;
    this.userServices.update(userId, this.formEditUser.getRawValue())
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
          MessageConstants.NOTIFICATION_UPDATE + this.formEditUser.get('userName').value + '!',
          'bottomRight');
        this.ngOnInit();
        setTimeout(() => {
          this.visibleEditUser = false;
        }, 500);
      }, error => {
        setTimeout(() => {
          this.visibleEditUser = false;
        }, 500);
      });
  }

  // Delete User

  delete(userName: string, userId: string) {
    this.userServices.delete(userId)
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
          MessageConstants.NOTIFICATION_DELETE + userName + ' !', 'bottomRight');
        this.ngOnInit();
      });
  }

  // Reset password

  showDeleteConfirm(userName: string, userId: string): void {
    this.confirmDeleteModal = this.modal.confirm({
      nzTitle: 'Do you Want to delete ' + userName + ' user ?',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzOnOk: () =>
        new Promise((resolve, reject) => {
          this.delete(userName, userId);
          setTimeout(Math.random() > 0.5 ? resolve : reject, 200);
        })
    });
  }

  showResetConfirm( userId: string): void {
    this.confirmDeleteModal = this.modal.confirm({
      nzTitle: 'Do you Want to reset password ' + userId + ' ?',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzOnOk: () =>
        new Promise((resolve, reject) => {
          this.restPassword(userId);
          setTimeout(Math.random() > 0.5 ? resolve : reject, 200);
        })
    });
  }

  restPassword(userId: string): void {
    this.userServices.resetUserPassword(userId)
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
        MessageConstants.NOTIFICATION_REST_PW + userId + '!', 'bottomRight');
      this.ngOnInit();
    });
  }

  // Notification

  createNotification(type: string, title: string, content: string, position: NzNotificationPlacement): void {
    this.notification.create(type, title, content, { nzPlacement: position });
  }

}
