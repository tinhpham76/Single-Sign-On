import { Injectable } from '@angular/core';
import { UserManager, UserManagerSettings, User, Profile } from 'oidc-client';
import { BehaviorSubject } from 'rxjs';
import { BaseService } from './base.service';


@Injectable({
  providedIn: 'root'
})
export class AuthService extends BaseService {

  // Observable navItem source
  private _authNavStatusSource = new BehaviorSubject<boolean>(false);
  // Observable navItem stream
  authNavStatus$ = this._authNavStatusSource.asObservable();

  private manager = new UserManager(getClientSettings());
  private user: User | null;

  constructor() {
    super();

    this.manager.getUser().then(user => {
      this.user = user;
      this._authNavStatusSource.next(this.isAuthenticated());
    });
  }

  login() {
    return this.manager.signinRedirect();
  }

  get profile(): Profile {
    return this.user != null ? this.user.profile : null;
  }

  async completeAuthentication() {
    this.user = await this.manager.signinRedirectCallback();
    this._authNavStatusSource.next(this.isAuthenticated());
  }

  isAuthenticated(): boolean {
    return this.user != null && !this.user.expired;
  }

  get authorizationHeaderValue(): string {
    if (this.user) {
      return `${this.user.token_type} ${this.user.access_token}`;
    }
    return null;
  }

  get name(): string {
    return this.user != null ? this.user.profile.name : '';
  }

  async signOut() {
    await this.manager.signoutRedirect();
  }
}

export function getClientSettings(): UserManagerSettings {
  return {
    authority: 'https://localhost:5000',
    client_id: 'angular_user_manager',
    redirect_uri: 'http://localhost:4300/auth-callback',
    post_logout_redirect_uri: 'http://localhost:4300/',
    response_type: 'code',
    scope: 'sso.api user.api openid profile',
    filterProtocolClaims: true,
    loadUserInfo: true,
    automaticSilentRenew: true,
    silent_redirect_uri: 'http://localhost:4300/silent-refresh.html'
  };
}
