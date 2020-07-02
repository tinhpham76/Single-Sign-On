import { Component, OnInit } from '@angular/core';
import { UserServices } from '@app/shared/services/users.services';
import { DatePipe } from '@angular/common';
import { NzNotificationService, NzNotificationPlacement } from 'ng-zorro-antd/notification';
import { AuthService } from '@app/shared/services/auth.service';
import { Subscription } from 'rxjs';
import { MessageConstants } from '@app/shared/constants/messages.constant';
import { User } from '@app/shared/models/user.model';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { NzModalService } from 'ng-zorro-antd/modal';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  public subscription: Subscription;
  public userId: string;
  public isAuthenticated: boolean;
  public isSpinning: boolean;

  // User detail
  public id: number;
  public userName: string;
  public firstName: string;
  public lastName: string;
  public email: string;
  public phoneNumber: number;
  public dob: string;


  panelUpdate = [
    {
      active: false,
      name: 'Update profile',
      disabled: false
    }
  ];
  panelChangePassword = [
    {
      active: false,
      name: 'Change password',
      disabled: false
    }
  ];
  panelDelete = [
    {
      active: false,
      name: 'Delete account',
      disabled: false
    }
  ];

  public formEditUser!: FormGroup;
  public formChangePassword!: FormGroup;

  constructor(private userServices: UserServices,
    private notification: NzNotificationService,
    private modal: NzModalService,
    private datePipe: DatePipe,
    private authServices: AuthService,
    private fb: FormBuilder) { }

  ngOnInit(): void {
    this.formChangePassword = this.fb.group({
      currentPassword: [null, [Validators.required]],
      newPassword: [null, [Validators.required]],
      checkPassword: [null, [Validators.required, this.confirmationValidator]],
    });
    this.formEditUser = this.fb.group({
      userName: [null],
      firstName: [null, [Validators.required]],
      lastName: [null, [Validators.required]],
      email: [null, [Validators.email, Validators.required]],
      phoneNumberPrefix: ['+84'],
      phoneNumber: [null, [Validators.required]],
      dob: [null, [Validators.required]]
    });
    this.subscription = this.authServices.authNavStatus$.subscribe(status => this.isAuthenticated = status);
    this.userId = this.authServices.profile.sub;
    this.getUser();
  }

  // Get user detail
  getUser(): void {
    this.isSpinning = true;
    this.userServices.getDetail(this.userId)
      .subscribe((res: User) => {
        this.userName = res.userName;
        this.firstName = res.firstName;
        this.lastName = res.lastName;
        this.email = res.email;
        this.phoneNumber = res.phoneNumber;
        this.dob = res.dob;
        this.formEditUser.setValue({
          userName: res.userName,
          firstName: res.firstName,
          lastName: res.lastName,
          email: res.email,
          phoneNumberPrefix: '+84',
          phoneNumber: res.phoneNumber,
          dob: res.dob
        });
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

  saveChanges() {
    this.isSpinning = true;
    this.userServices.update(this.userId, this.formEditUser.getRawValue())
      .subscribe(() => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_UPDATE + this.formEditUser.get('userName').value + '!',
          'bottomRight');
        this.ngOnInit();
        setTimeout(() => {
          this.getUser();
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
          this.getUser();
          this.isSpinning = false;
        }, 500);
      });
  }

  // Validator
  confirmationValidator = (control: FormControl): { [s: string]: boolean } => {
    if (!control.value) {
      return { required: true };
    } else if (control.value !== this.formChangePassword.controls.newPassword.value) {
      return { confirm: true, error: true };
    }
    return {};
  }

  updateConfirmValidator(): void {
    /** wait for refresh value */
    Promise.resolve().then(() => this.formChangePassword.controls.checkPassword.updateValueAndValidity());
  }

  changePassword(): void {
    this.isSpinning = true;
    this.userServices.changePassword(this.userId, this.formChangePassword.getRawValue())
      .subscribe(() => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_UPDATE + ' password ' + this.formEditUser.get('userName').value + '!',
          'bottomRight');
        this.ngOnInit();
        setTimeout(() => {
          this.getUser();
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
          this.getUser();
          this.isSpinning = false;
        }, 500);
      });

  }

  delete(): void {
    this.modal.confirm({
      nzTitle: 'Are you sure delete this account?',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzOnOk: () => this.deleteAccount(),
      nzCancelText: 'No',
      nzOnCancel: () => console.log('Cancel')
    });
  }

  deleteAccount(): void {
    this.userServices.delete(this.userId)
      .subscribe(() => {
        this.authServices.signOut();
      }, errorMessage => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_ERROR,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          errorMessage,
          'bottomRight'
        );
        setTimeout(() => {
          this.getUser();
          this.isSpinning = false;
        }, 500);
      });
  }


  // Notification
  createNotification(type: string, title: string, content: string, position: NzNotificationPlacement): void {
    this.notification.create(type, title, content, { nzPlacement: position });
  }

}
