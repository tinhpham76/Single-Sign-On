import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserServices } from '@app/shared/services/users.services';
import { NzNotificationService, NzNotificationPlacement } from 'ng-zorro-antd/notification';
import { MessageConstants } from '@app/shared/constants/messages.constant';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { RolesServices } from '@app/shared/services/role.service';
import { FormGroup, FormBuilder } from '@angular/forms';
import { User } from '@app/shared/models/user.model';

@Component({
  selector: 'app-user-role',
  templateUrl: './user-role.component.html',
  styleUrls: ['./user-role.component.scss']
})
export class UserRoleComponent implements OnInit {

  // load roles
  public roles = [];

  // load data
  public userId: string;
  public userName: string;
  public email: string;

  // Spin
  public isSpinning: boolean;

  constructor(private route: ActivatedRoute,
    private userServices: UserServices,
    private notification: NzNotificationService) { }


  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.userId = params['userId'];
      this.getUserRoles(params['userId']);
    });
    this.loadUserDetail(this.userId);
  }

  // Load user detail
  loadUserDetail(userId: string) {
    this.isSpinning = true;
    this.userServices.getDetail(userId)
      .subscribe((res: User) => {
        this.userName = res.userName;
        this.email = res.email;
        this.isSpinning = false;
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

  // Load user role
  getUserRoles(userId: string) {
    this.isSpinning = true;
    this.userServices.getUserRoles(userId)
      .subscribe((res: any[]) => {
        this.roles = res;
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

  // Notification
  createNotification(type: string, title: string, content: string, position: NzNotificationPlacement): void {
    this.notification.create(type, title, content, { nzPlacement: position });
  }

  // Event
  event(value: string, roleNames: string): void {
    this.isSpinning = true;
    if (value === null || value === undefined || value === ' ' || value.length === 0) {
      this.userServices.removeRolesFromUser(this.userId, [roleNames])
        .subscribe(() => {
          this.createNotification(
            MessageConstants.TYPE_NOTIFICATION_SUCCESS,
            MessageConstants.TITLE_NOTIFICATION_SSO,
            MessageConstants.NOTIFICATION_DELETE + this.userId + ' role!', 'bottomRight');
          setTimeout(() => {
            this.ngOnInit();
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
    } else if (roleNames === 'Admin') {
      this.createNotification(
        MessageConstants.TYPE_NOTIFICATION_WARNING,
        MessageConstants.TITLE_NOTIFICATION_SSO,
        'Role Admin cannot delete!',
        'bottomRight');
      setTimeout(() => {
        this.ngOnInit();
        this.isSpinning = false;
      }, 1000);
    } else {
      const selectedNames = [roleNames];
      const assignRolesToUser = { roleNames: selectedNames };
      this.isSpinning = true;
      this.userServices.assignRolesToUser(this.userId, assignRolesToUser)
        .subscribe(() => {
          this.createNotification(
            MessageConstants.TYPE_NOTIFICATION_SUCCESS,
            MessageConstants.TITLE_NOTIFICATION_SSO,
            MessageConstants.NOTIFICATION_ADD + this.userId + ' roles!', 'bottomRight');
          setTimeout(() => {
            this.ngOnInit();
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
}

