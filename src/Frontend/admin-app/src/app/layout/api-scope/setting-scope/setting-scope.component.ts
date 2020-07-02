import { Component, OnInit } from '@angular/core';
import { NzNotificationService, NzNotificationPlacement } from 'ng-zorro-antd/notification';
import { ActivatedRoute } from '@angular/router';
import { ApiScopeServices } from '@app/shared/services/api-scope.services';
import { MessageConstants } from '@app/shared/constants/messages.constant';
import { ApiScope } from '@app/shared/models/api-scope.model';
import { RolesServices } from '@app/shared/services/role.service';

@Component({
  selector: 'app-setting-scope',
  templateUrl: './setting-scope.component.html',
  styleUrls: ['./setting-scope.component.scss']
})
export class SettingScopeComponent implements OnInit {
  public name: string;

  // Spin
  public isSpinning = true;

  // Claim
  public apiClaimTags = [];
  public tempClaim = [];
  public temp = []

  // Identity resource detail
  public displayName: string;
  public description: string;
  public enabled: boolean;

  constructor(private apiScopeServices: ApiScopeServices,
    private roleServices: RolesServices,
    private notification: NzNotificationService,
    private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.name = params['name'];
      this.getApiScopeDetail(params['name']);
      this.getApiScopeClaims(params['name']);
    });
  }

  // Get api scope detail
  getApiScopeDetail(name: string): void {
    this.isSpinning = true;
    this.apiScopeServices.getDetail(name)
      .subscribe((res: ApiScope) => {
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



  // Get api scope
  getApiScopeClaims(name: string): void {
    this.roleServices.getAll()
    .subscribe((res: any[]) => {
      this.tempClaim = res;
    });
    this.isSpinning = true;
    this.apiScopeServices.getApiScopeClaims(name)
      .subscribe((res: any[]) => {
        this.apiClaimTags = res;
        res.forEach(type => {
          for (let i = 0; i < this.tempClaim.length; i++) {
            if (this.tempClaim[i].toString() === type.toString()) {
              this.tempClaim.splice(Number(i), 1);
            }
          }
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

  // Add new api scope claim
  addApiScopeClaim(type: string): void {
    this.isSpinning = true;
    const data = Object.assign({ type });
    this.apiScopeServices.addApiScopeClaim(this.name, data)
      .subscribe(() => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_ADD + type + ' claim for ' + this.name + ' resource!',
          'bottomRight');
        setTimeout(() => {
          this.getApiScopeClaims(this.name);
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

  // delete claim of api scope
  deleteApiScopeClaim(tag: string) {
    this.isSpinning = true;
    this.apiScopeServices.deleteApiScopeClaim(this.name, tag)
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
          this.getApiScopeClaims(this.name);
          this.isSpinning = false;    
        }, 500);
      });
  }

  // notification
  createNotification(type: string, title: string, content: string, position: NzNotificationPlacement): void {
    this.notification.create(type, title, content, { nzPlacement: position });
  }

}

