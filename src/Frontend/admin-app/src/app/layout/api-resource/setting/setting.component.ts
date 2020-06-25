import { Component, OnInit } from '@angular/core';
import { ApiResourceServices } from '@app/shared/services/api-resources.service';
import { NzNotificationService, NzNotificationPlacement } from 'ng-zorro-antd/notification';
import { ActivatedRoute } from '@angular/router';
import { catchError } from 'rxjs/operators';
import { MessageConstants } from '@app/shared/constants/messages.constant';
import { throwError } from 'rxjs';
import { ApiResource } from '@app/shared/models/api-resource.model';

@Component({
  selector: 'app-setting',
  templateUrl: './setting.component.html',
  styleUrls: ['./setting.component.scss']
})
export class SettingComponent implements OnInit {

  //
  public name: string;

  // Spin
  public isSpinning = true;

  // Claim
  public apiClaimTags = [];
  public tempClaim = [];

  // Identity resource detail
  public displayName: string;
  public description: string;
  public enabled: boolean;

  constructor(private apiResourceServices: ApiResourceServices,
    private notification: NzNotificationService,
    private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.name = params['name'];
      this.getApiResourceDetail(params['name']);
      this.getApiResourceClaims(params['name']);
    });
  }

  // Get Identity resource detail
  getApiResourceDetail(name: string): void {
    this.isSpinning = true;
    this.apiResourceServices.getDetail(name)
      .subscribe((res: ApiResource) => {
        this.displayName = res.displayName,
          this.description = res.description,
          this.enabled = res.enabled,
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

  // Get claims for identity resource
  getApiResourceClaims(name: string): void {
    this.isSpinning = true;
    this.apiResourceServices.getApiResourceClaims(name)
      .subscribe((res: any[]) => {
        this.apiClaimTags = res;
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

  // Add new claim for identity resource
  addApiResourceClaim(type: string): void {
    this.isSpinning = true;
    const data = Object.assign({ type });
    this.apiResourceServices.addApiResourceClaim(this.name, data)
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

  // delete claim of identity resource
  deleteApiResourceClaim(tag: string) {
    this.isSpinning = true;
    this.apiResourceServices.deleteApiResourceClaim(this.name, tag)
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

  // notification
  createNotification(type: string, title: string, content: string, position: NzNotificationPlacement): void {
    this.notification.create(type, title, content, { nzPlacement: position });
  }

}
