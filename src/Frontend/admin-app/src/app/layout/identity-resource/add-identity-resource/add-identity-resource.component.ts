import { Component, OnInit } from '@angular/core';
import { UserServices } from '@app/shared/services/users.services';
import { NzNotificationService, NzNotificationPlacement } from 'ng-zorro-antd/notification';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageConstants } from '@app/shared/constants/messages.constant';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { IdentityResourceServices } from '@app/shared/services/identity-resources.service';

@Component({
  selector: 'app-add-identity-resource',
  templateUrl: './add-identity-resource.component.html',
  styleUrls: ['./add-identity-resource.component.scss']
})
export class AddIdentityResourceComponent implements OnInit {

  // Init form
  public validateForm!: FormGroup;

  // Spin
  public isSpinning: boolean;

  constructor(private fb: FormBuilder,
    private identityResourceServices: IdentityResourceServices,
    private notification: NzNotificationService,
    private router: Router) { }

  ngOnInit(): void {
    this.validateForm = this.fb.group({
      name: [null, [Validators.required]],
      displayName: [null, Validators.required],
      description: [null],
      enabled: [true],
      showInDiscoveryDocument: [true],
      required: [false],
      emphasize: [false]
    });
  }

  // Create new identity resource
  submitForm(): void {
    const data = this.validateForm.getRawValue();
    this.identityResourceServices.add(data)
      .subscribe(() => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_ADD,
          'bottomRight');
        setTimeout(() => {
          this.router.navigate(['/identity-resources']);
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

}
