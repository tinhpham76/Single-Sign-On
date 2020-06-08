import { Component, OnInit } from '@angular/core';
import { routerTransition } from '../router.animations';
import { NgxSpinnerService } from 'ngx-spinner';
import { AuthService } from '@app/shared/services/auth.service';
import { ToastrService } from 'ngx-toastr';

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
    private toastr: ToastrService
  ) { }

  ngOnInit() { }

  loginWithSSO() {
    this.spinner.show();
    this.authService.login();
  }
  loginWithFacebook() {
    this.toastr.error('The service has not been registered', 'Facebook!');
  }
  loginWithGoogle() {
    this.toastr.error('The service has not been registered', 'Google!');
  }
  login(){
    this.toastr.warning('Please login with SSO');
  }
}
