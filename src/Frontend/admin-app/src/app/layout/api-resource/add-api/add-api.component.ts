import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ApiResourceServices } from '@app/shared/services/api-resources.service';
import { NzNotificationService, NzNotificationPlacement } from 'ng-zorro-antd/notification';
import { Router } from '@angular/router';
import { catchError } from 'rxjs/operators';
import { MessageConstants } from '@app/shared/constants/messages.constant';
import { throwError } from 'rxjs';

@Component({
  selector: 'app-add-api',
  templateUrl: './add-api.component.html',
  styleUrls: ['./add-api.component.scss']
})
export class AddApiComponent implements OnInit {

  // Init form
  public validateForm!: FormGroup;

  constructor(private fb: FormBuilder,
    private apiResourceServices: ApiResourceServices,
    private notification: NzNotificationService,
    private router: Router) { }

  ngOnInit(): void {
    this.validateForm = this.fb.group({
      name: [null, [Validators.required]],
      displayName: [null, Validators.required],
      description: [null],
      enabled: [true],
    });
  }

  // Create new identity resource
  submitForm(): void {
    const data = this.validateForm.getRawValue();
    this.apiResourceServices.add(data)
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
          MessageConstants.NOTIFICATION_ADD,
          'bottomRight');
        setTimeout(() => {
          this.router.navigate(['/api-resources']);
        }, 500);
      },
        error => {
          setTimeout(() => {
          }, 500);
        });
  }

  // Notification
  createNotification(type: string, title: string, content: string, position: NzNotificationPlacement): void {
    this.notification.create(type, title, content, { nzPlacement: position });
  }
}