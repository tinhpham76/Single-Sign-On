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
  public items: any[];
  public apiName: string;
  public name: string;
  public description: string;
  public displayName: string;
  public required: boolean;
  public showInDiscoveryDocument: boolean;
  public emphasize: boolean;
  public scopeName: string;

  // Claim
  public apiClaimTags = [];
  public tempClaim = [];

  // Spin
  public isSpinning: boolean;
  public isSpinningDrawer: boolean;

  // Init form
  validateForm!: FormGroup;

  // Drawer Edit user
  visibleApi = false;

  // Modal
  confirmDeleteModal?: NzModalRef;

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
    this.validateForm = this.fb.group({
      name: [null, [Validators.required]],
      displayName: [null, Validators.required],
      description: [null],
      required: [false],
      showInDiscoveryDocument: [true],
      emphasize: [true]
    });
    this.getApiResourceScope(this.apiName);
  }

  // Button go back
  goBack() {
    this.location.back();
  }

  // Get api scope
  getApiResourceScope(apiName: string) {
    this.isSpinning = true;
    this.apiResourceServices.getApiResourceScope(apiName)
      .pipe(catchError(err => {
        this.isSpinning = true;
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_ERROR,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_ERROR,
          'bottomRight'
        );
        return throwError('Error', err);
      }))
      .subscribe((res: any[]) => {
        this.items = res;
        setTimeout(() => {
          this.isSpinning = false;
        }, 500);
      });
  }

  // Open api scope detail
  openApiResourceScope(name: string, description: string,
    displayName: string,
    required: boolean,
    showInDiscoveryDocument: boolean,
    emphasize: boolean): void {
    this.scopeName = name;
    this.getApiResourceScopeClaims(name);
    this.visibleApi = true;
    this.name = name;
    this.description = description;
    this.displayName = displayName;
    this.required = required;
    this.showInDiscoveryDocument = showInDiscoveryDocument;
    this.emphasize = emphasize;
  }

  closeEditApiResource(): void {
    this.visibleApi = false;
    this.ngOnInit();
  }

  // Get claims for api resource scope claims
  getApiResourceScopeClaims(scopeName: string): void {
    this.isSpinningDrawer = true;
    this.apiResourceServices.getApiResourceScopeClaim(this.apiName, scopeName)
      .pipe(
        catchError(err => {
          this.createNotification(
            MessageConstants.TYPE_NOTIFICATION_ERROR,
            MessageConstants.TITLE_NOTIFICATION_SSO,
            MessageConstants.NOTIFICATION_ERROR,
            'bottomRight'
          );
          return throwError('Error: ', err);
        }))
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
          this.isSpinningDrawer = false;
        }, 500);
      });
  }

  // Add new claim for api scope claim
  addApiResourceClaim(type: string): void {
    this.isSpinningDrawer = true;
    const data = Object.assign({ type });
    this.apiResourceServices.addApiResourceScopeClaim(this.apiName, this.name, data)
      .pipe(catchError(err => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_ERROR,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_ERROR,
          'bottomRight'
        );
        return throwError('Error', err);
      }))
      .subscribe(() => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_ADD + type + ' claim for ' + this.name + ' resource!',
          'bottomRight');
        setTimeout(() => {
          this.getApiResourceScopeClaims(this.scopeName);
          this.isSpinningDrawer = false;
        }, 1000);
      });

  }



  // Create new api resource scope
  submitForm(): void {
    this.isSpinning = true;
    const data = this.validateForm.getRawValue();
    this.apiResourceServices.addApiResourceScope(this.apiName, data)
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
        this.ngOnInit();
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_ADD + name + '!',
          'bottomRight');
      });
  }

  // Delete api scopes
  delete(scopeName: string) {
    this.isSpinning = true;
    this.apiResourceServices.deleteApiResourceScope(this.apiName, scopeName)
      .pipe(catchError(err => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_ERROR,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_ERROR,
          'bottomRight'
        );
        return throwError('Error', err);
      }))
      .subscribe(res => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_DELETE + name + ' !', 'bottomRight');
        this.ngOnInit();
        setTimeout(() => {
          this.isSpinning = false;
        }, 500);
      });
  }

  // Delete api scope
  showDeleteConfirm(name: string): void {
    this.confirmDeleteModal = this.modal.confirm({
      nzTitle: 'Do you Want to delete api resource scope' + name + ' ?',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzOnOk: () =>
        new Promise((resolve, reject) => {
          this.delete(name);
          setTimeout(Math.random() > 0.5 ? resolve : reject, 200);
        })
    });
  }


  // Notification
  createNotification(type: string, title: string, content: string, position: NzNotificationPlacement): void {
    this.notification.create(type, title, content, { nzPlacement: position });
  }
}
