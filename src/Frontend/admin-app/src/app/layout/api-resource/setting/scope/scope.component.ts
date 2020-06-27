import { Component, OnInit } from '@angular/core';
import { ApiResourceServices } from '@app/shared/services/api-resources.service';
import { NzNotificationService, NzNotificationPlacement } from 'ng-zorro-antd/notification';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { catchError } from 'rxjs/operators';
import { MessageConstants } from '@app/shared/constants/messages.constant';
import { throwError } from 'rxjs';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NzModalRef, NzModalService } from 'ng-zorro-antd/modal';

@Component({
  selector: 'app-scope',
  templateUrl: './scope.component.html',
  styleUrls: ['./scope.component.scss']
})
export class ScopeComponent implements OnInit {

  // Scope claim
  public apiName: string;
  public name: string;
  public description: string;
  public displayName: string;
  public required: boolean;
  public showInDiscoveryDocument: boolean;
  public emphasize: boolean;
  public scopeName: string;

  // Check box scope
  public scopes = [];


  // Spin
  public isSpinning: boolean;

  constructor(private apiResourceServices: ApiResourceServices,
    private notification: NzNotificationService,
    private route: ActivatedRoute,
    private location: Location,
    private modal: NzModalService,
    private fb: FormBuilder) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.apiName = params['name'];
    });
    this.getApiResourceScope(this.apiName);
  }


  // Get api resource scope
  getApiResourceScope(apiName: string) {
    this.isSpinning = true;
    this.apiResourceServices.getApiResourceScope(apiName)
      .subscribe((res: any[]) => {
        this.scopes = res;
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


  event(value: string, scope: string): void {
    this.isSpinning = true;
    if (value === null || value === undefined || value === ' ' || value.length === 0) {
      this.apiResourceServices.deleteApiResourceScope(this.apiName, scope)
        .subscribe(() => {
          this.createNotification(
            MessageConstants.TYPE_NOTIFICATION_SUCCESS,
            MessageConstants.TITLE_NOTIFICATION_SSO,
            MessageConstants.NOTIFICATION_DELETE + this.apiName + ' scope!', 'bottomRight');
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
            this.ngOnInit();
            this.isSpinning = false;
          }, 500);
        });
    } else {
      this.isSpinning = true;
      this.apiResourceServices.addApiResourceScope(this.apiName, { scope })
        .subscribe(() => {
          this.createNotification(
            MessageConstants.TYPE_NOTIFICATION_SUCCESS,
            MessageConstants.TITLE_NOTIFICATION_SSO,
            MessageConstants.NOTIFICATION_ADD + this.apiName + ' roles!', 'bottomRight');
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
            this.ngOnInit();
            this.isSpinning = false;
          }, 500);
        });
    }
  }

  // Notification
  createNotification(type: string, title: string, content: string, position: NzNotificationPlacement): void {
    this.notification.create(type, title, content, { nzPlacement: position });
  }
}
