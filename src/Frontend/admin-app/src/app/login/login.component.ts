import { Component, OnInit } from '@angular/core';
import { routerTransition } from '../router.animations';
import { NgxSpinnerService } from 'ngx-spinner';
import { AuthService } from '@app/shared/services/auth.service';
import { NzNotificationService } from 'ng-zorro-antd/notification';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  animations: [routerTransition()]
})
export class LoginComponent implements OnInit {
  constructor(
    private authService: AuthService,
    private spinner: NgxSpinnerService,
    private notification: NzNotificationService
  ) { }

  ngOnInit() { }

  loginWithIS4() {
    this.spinner.show();
    this.authService.login();
  }
  createNotification(type: string, title: string, content: string): void {
    this.notification.create( type, title, content);
  }
}
