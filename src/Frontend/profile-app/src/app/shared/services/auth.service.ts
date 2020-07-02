import { Injectable } from '@angular/core';
import { UserManager, UserManagerSettings, User, Profile } from 'oidc-client';
import { BehaviorSubject } from 'rxjs';
import { BaseService } from './base.service';
import { environment } from '@environments/environment';


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
    authority: environment.authority,
    client_id: environment.client_id,
    redirect_uri: environment.redirect_uri,
    post_logout_redirect_uri: environment.post_logout_redirect_uri,
    response_type: 'code',
    scope: environment.scope,
    filterProtocolClaims: true,
    loadUserInfo: true,
    automaticSilentRenew: true,
    silent_redirect_uri: environment.silent_redirect_uri
  };
}
