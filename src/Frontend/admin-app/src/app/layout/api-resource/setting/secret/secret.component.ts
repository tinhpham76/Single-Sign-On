import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NzModalRef, NzModalService } from 'ng-zorro-antd/modal';
import { ApiResourceServices } from '@app/shared/services/api-resources.service';
import { NzNotificationService, NzNotificationPlacement } from 'ng-zorro-antd/notification';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { catchError } from 'rxjs/operators';
import { MessageConstants } from '@app/shared/constants/messages.constant';
import { throwError } from 'rxjs';

@Component({
  selector: 'app-secret',
  templateUrl: './secret.component.html',
  styleUrls: ['./secret.component.scss']
})
export class SecretComponent implements OnInit {

  //
  public apiName: string;
  public items: any[];

  // Spin
  public isSpinning: boolean;

  // Init form
  validateForm!: FormGroup;

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
      type: ['SharedSecret'],
      value: [null, Validators.required],
      description: [null],
      expiration: [null],
      hashType: ['Sha256'],
    });
    this.getApiResourceSecret(this.apiName);
  }

  // Button go back
  goBack() {
    this.location.back();
  }

  // Get api secret
  getApiResourceSecret(apiName: string) {
    this.isSpinning = true;
    this.apiResourceServices.getApiResourceSecret(apiName)
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


  // Create new api resource scope
  submitForm(): void {
    this.isSpinning = true;
    const data = this.validateForm.getRawValue();
    this.apiResourceServices.addApiResourceSecret(this.apiName, data)
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
        this.ngOnInit();
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_ADD + name + '!',
          'bottomRight');
      });
  }

  // Delete api scopes
  delete(id: string) {
    this.isSpinning = true;
    this.apiResourceServices.deleteApiResourceSecret(this.apiName, Number(id))
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
  showDeleteConfirm(id: string): void {
    this.confirmDeleteModal = this.modal.confirm({
      nzTitle: 'Do you Want to delete api resource scope secrets' + id + ' ?',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzOnOk: () =>
        new Promise((resolve, reject) => {
          this.delete(id);
          setTimeout(Math.random() > 0.5 ? resolve : reject, 200);
        })
    });
  }


  // Notification
  createNotification(type: string, title: string, content: string, position: NzNotificationPlacement): void {
    this.notification.create(type, title, content, { nzPlacement: position });
  }

}
