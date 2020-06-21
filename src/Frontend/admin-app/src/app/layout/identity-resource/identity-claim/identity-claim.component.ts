import { Component, OnInit } from '@angular/core';
import { NzNotificationPlacement, NzNotificationService } from 'ng-zorro-antd/notification';
import { NzModalRef } from 'ng-zorro-antd/modal';
import { IdentityResourceServices } from '@app/shared/services/identity-resources.service';
import { catchError } from 'rxjs/operators';
import { MessageConstants } from '@app/shared/constants/messages.constant';
import { throwError } from 'rxjs';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-identity-claim',
  templateUrl: './identity-claim.component.html',
  styleUrls: ['./identity-claim.component.scss']
})
export class IdentityClaimComponent implements OnInit {

  //
  public name: string;

  // Spin
  public isSpinning = true;

  // Claim
  public identityClaimTags = [];
  public tempClaim = [];

  constructor(private identityResourceServices: IdentityResourceServices,
    private notification: NzNotificationService,
    private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.name = params['name'];
      this.getIdentityResourceClaims(params['name']);
    });
  }

  // Get claims for identity resource
  getIdentityResourceClaims(name: string): void {
    this.isSpinning = true;
    this.identityResourceServices.getIdentityResourceClaims(name)
      .pipe(
        catchError(err => {
          this.createNotification(
            MessageConstants.NOTIFICATION_ERROR,
            MessageConstants.TITLE_NOTIFICATION_SSO,
            MessageConstants.NOTIFICATION_ERROR,
            'bottomRight'
          );
          return throwError('Error: ', err);
        }))
      .subscribe((res: any[]) => {
        this.identityClaimTags = res;
        const temp = ['sub', 'name', 'given_name', 'family_name', 'middle_name',
          'nickname', 'preferred_username', 'profile', 'picture', 'website', 'email', 'email_verified',
          'gender', 'birthdate', 'zoneinfo', 'locale', 'phone_number', 'phone_number_verified', 'address', 'updated_at'];
        res.forEach(type => {
          for (let i = 0; i < temp.length; i++) {
            if (temp[i].toString() === type.toString()) {
              temp.splice(Number(i), 1);
            }
          }
        });
        this.tempClaim = temp;
        setTimeout(() => {
          this.isSpinning = false;
        }, 500);
      });
  }

  // Add new claim for identity resource
  addIdentityResourceClaim(type: string): void {
    this.isSpinning = true;
    const data = Object.assign({ type });
    this.identityResourceServices.addIdentityResourceClaim(this.name, data)
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
          MessageConstants.NOTIFICATION_ADD + type + ' claim for ' + this.name + ' resource!',
          'bottomRight');
        setTimeout(() => {
          this.ngOnInit();
          this.isSpinning = false;
        }, 500);
      });

  }

  // delete claim of identity resource
  deleteIdentityResourceClaim(tag: string) {
    this.isSpinning = true;
    this.identityResourceServices.deleteIdentityResourceClaim(this.name, tag)
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
          MessageConstants.NOTIFICATION_DELETE + tag + ' claim of ' + this.name + ' resource!',
          'bottomRight');
        setTimeout(() => {
          this.ngOnInit();
          this.isSpinning = false;
        }, 500);
      });
  }

  // notification
  createNotification(type: string, title: string, content: string, position: NzNotificationPlacement): void {
    this.notification.create(type, title, content, { nzPlacement: position });
  }

}

