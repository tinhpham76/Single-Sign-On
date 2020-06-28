import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from '@app/shared/services/auth.service';

@Component({
  selector: 'app-auth-callback',
  templateUrl: './auth-callback.component.html',
  styleUrls: ['./auth-callback.component.css']
})
export class AuthCallbackComponent implements OnInit {

  error: boolean;

  constructor(private authService: AuthService, private router: Router, private route: ActivatedRoute) { }

  async ngOnInit() {

    // check for error
    if (this.route.snapshot.queryParams.error) {
      this.error = true;
      return;
    }

    await this.authService.completeAuthentication();
    this.router.navigate(['/']);
  }
}
