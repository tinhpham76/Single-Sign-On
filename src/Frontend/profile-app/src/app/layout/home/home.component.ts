import { Component, OnInit } from '@angular/core';
import { UserServices } from '@app/shared/services/users.services';
import { DatePipe } from '@angular/common';
import { NzNotificationService, NzNotificationPlacement } from 'ng-zorro-antd/notification';
import { AuthService } from '@app/shared/services/auth.service';
import { Subscription } from 'rxjs';
import { MessageConstants } from '@app/shared/constants/messages.constant';
import { User } from '@app/shared/models/user.model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  public subscription: Subscription;
  public userId: string;
  public isAuthenticated: boolean;
  public isSpinning: boolean;

  // User detail
  public id: number;
  public userName: string;
  public firstName: string;
  public lastName: string;
  public email: string;
  public phoneNumber: number;
  public dob: Date;
  public createDate: Date;
  public lastModifiedDate: Date;

  panelUpdate = [
    {
      active: false,
      name: 'Update profile',
      disabled: false
    }
  ];

  //public formEditUser!: FormGroup;

  constructor(private userServices: UserServices,
    private notification: NzNotificationService,
   /* private modal: NzModalService,*/
    private datePipe: DatePipe,
    private authServices: AuthService,
   /* private fb: FormBuilder*/) { }

  ngOnInit(): void {
    this.isSpinning = true;
    /*this.formEditUser = this.fb.group({
      id: [null],
      userName: [null],
      firstName: [null, [Validators.required]],
      lastName: [null, [Validators.required]],
      email: [null, [Validators.email, Validators.required]],
      phoneNumberPrefix: ['+84'],
      phoneNumber: [null, [Validators.required]],
      dob: [null, [Validators.required]]
    });*/
    this.subscription = this.authServices.authNavStatus$.subscribe(status => this.isAuthenticated = status);
    this.userId = this.authServices.profile.sub;
    this.userServices.get()
    .subscribe((res: User) => {
      console.log(res);
      
     /* this.formEditUser.setValue({
        id: res.id,
        userName: res.userName,
        firstName: res.firstName,
        lastName: res.lastName,
        email: res.email,
        phoneNumberPrefix: '+84',
        phoneNumber: res.phoneNumber,
        dob: dob
      });*/
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
    setTimeout(() => {
      this.isSpinning = false;
    }, 5000);

    

  }


  // Notification
  createNotification(type: string, title: string, content: string, position: NzNotificationPlacement): void {
    this.notification.create(type, title, content, { nzPlacement: position });
  }

}
