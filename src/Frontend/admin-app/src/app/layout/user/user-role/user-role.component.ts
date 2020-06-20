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

  roles = [];
  userId: string;
  isSpinning = true;
  userName: string;
  email: string;

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

  loadUserDetail(userId: string){
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
        this.userName = res.userName;
        this.email = res.email;
      });
  }

  getUserRoles(userId: string) {
    this.userServices.getUserRoles(userId)
      .pipe(catchError(err => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_ERROR,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_ERROR,
          'bottomRight'
        );
        setTimeout(() => {
          this.isSpinning = false;
          this.ngOnInit();
        }, 1000);
        return throwError('Error');
      }))
      .subscribe((res: any[]) => {
        this.roles = res;
        setTimeout(() => {
          this.isSpinning = false;
        }, 1000);
      });
  }

  createNotification(type: string, title: string, content: string, position: NzNotificationPlacement): void {
    this.notification.create(type, title, content, { nzPlacement: position });
  }

  event(value: string, roleNames: string): void {
    if (value === null || value === undefined || value === ' ' || value.length === 0) {
      this.userServices.removeRolesFromUser(this.userId, [roleNames])
        .pipe(catchError(err => {
          this.createNotification(
            MessageConstants.TYPE_NOTIFICATION_ERROR,
            MessageConstants.TITLE_NOTIFICATION_SSO,
            MessageConstants.NOTIFICATION_ERROR,
            'bottomRight'
          );
          setTimeout(() => {
            this.isSpinning = false;
            this.ngOnInit();
          }, 1000);
          return throwError('Error');
        }))
        .subscribe(() => {
          this.createNotification(
            MessageConstants.TYPE_NOTIFICATION_SUCCESS,
            MessageConstants.TITLE_NOTIFICATION_SSO,
            MessageConstants.NOTIFICATION_DELETE +  this.userId + ' role!', 'bottomRight');
            setTimeout(() => {
              this.isSpinning = false;
              this.ngOnInit();
            }, 1000);
        });
    } else if (roleNames === 'Admin') {
      this.createNotification(
        MessageConstants.TYPE_NOTIFICATION_WARNING,
        MessageConstants.TITLE_NOTIFICATION_SSO,
        MessageConstants.NOTIFICATION_ROLE_AD + ' !', 'bottomRight');
        setTimeout(() => {
          this.isSpinning = false;
          this.ngOnInit();
        }, 1000);
    } else {
      const selectedNames = [roleNames];
      const assignRolesToUser = {
        roleNames: selectedNames
      };
      this.userServices.assignRolesToUser(this.userId, assignRolesToUser)
        .pipe(catchError(err => {
          this.createNotification(
            MessageConstants.TYPE_NOTIFICATION_ERROR,
            MessageConstants.TITLE_NOTIFICATION_SSO,
            MessageConstants.NOTIFICATION_ERROR,
            'bottomRight'
          );
          setTimeout(() => {
            this.isSpinning = false;
            this.ngOnInit();
          }, 1000);
          return throwError('Error');
        }))
        .subscribe(() => {
          this.createNotification(
            MessageConstants.TYPE_NOTIFICATION_SUCCESS,
            MessageConstants.TITLE_NOTIFICATION_SSO,
            MessageConstants.NOTIFICATION_ADD + this.userId + ' roles!', 'bottomRight');
            setTimeout(() => {
              this.isSpinning = false;
              this.ngOnInit();
            }, 1000);
        });
    }
  }
}

