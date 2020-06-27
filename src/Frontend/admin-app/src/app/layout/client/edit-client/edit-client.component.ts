import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { NzNotificationService, NzNotificationPlacement } from 'ng-zorro-antd/notification';
import { ActivatedRoute } from '@angular/router';
import { ClientServices } from '@app/shared/services/clients.service';
import { MessageConstants } from '@app/shared/constants/messages.constant';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { FormGroup, FormBuilder, Validators, Form } from '@angular/forms';
import { isNumber } from 'util';
import { NzModalService, NzModalRef } from 'ng-zorro-antd/modal';

@Component({
  selector: 'app-edit-client',
  templateUrl: './edit-client.component.html',
  styleUrls: ['./edit-client.component.scss']
})
export class EditClientComponent implements OnInit {

  // Spin
  public isSpinning: boolean;

  // Client
  public clientId: string;
  public clientName: string;
  public clientUri: string;

  // Form
  public basicForm!: FormGroup;
  public settingForm!: FormGroup;
  public authenticationForm!: FormGroup;
  public tokenForm!: FormGroup;
  public deviceFlowForm!: FormGroup;
  public validateFormClientSecrets!: FormGroup;
  public validateFormClientClaims!: FormGroup;

  // Tags origin
  public originTags = [];
  inputVisibleOrigin = false;
  inputValueOrigin = '';

  // Tags Redirect Uri
  public redirectUriTags = [];
  inputVisibleRedirectUri = false;
  inputValueRedirectUri = '';

  // Tags scopes
  public allowedScopes = [];
  public scopeTags = [];

  // Tags Allowed Grant Types
  public allowedGrantTypes = [];
  public grantTypes = [
    'authorization_code',
    'client_credentials',
    'refresh_token',
    'implicit',
    'password',
    'urn:ietf:params:oauth:grant-type:device_code'];

  // Tags Post Logout Redirect Uris
  public postLogoutRedirectUris = [];
  inputVisibleLogoutUri = false;
  inputValueLogoutUri = '';

  // Drawer
  public visibleClientSecrets = false;
  public visibleClientClaims = false;

  // Client Drawer
  public itemClientSecrets: any[];
  public itemClientClaims: any[];

  // Modal
  confirmDeleteClientSecret?: NzModalRef;
  confirmDeleteClientClaim?: NzModalRef;

  // Select
  public claimTypeSelects = ['sub', 'name', 'given_name', 'family_name', 'middle_name',
    'nickname', 'preferred_username', 'profile', 'picture', 'website', 'email', 'email_verified',
    'gender', 'birthdate', 'zoneinfo', 'locale', 'phone_number', 'phone_number_verified', 'address', 'updated_at'];

  @ViewChild('inputElement', { static: false }) inputElement?: ElementRef;

  constructor(private notification: NzNotificationService,
    private route: ActivatedRoute,
    private clientServices: ClientServices,
    private fb: FormBuilder,
    private modal: NzModalService) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.clientId = params['id'];
    });

    // Init form basic
    this.basicForm = this.fb.group({
      clientId: [null, [Validators.required]],
      clientName: [null, [Validators.required]],
      description: [null],
      clientUri: [null, [Validators.required]],
      logoUri: [null],
    });

    // Init form setting
    this.settingForm = this.fb.group({
      enabled: [null],
      requireConsent: [null],
      allowRememberConsent: [null],
      allowOfflineAccess: [null],
      requireClientSecret: [null],
      protocolType: [null],
      requirePkce: [null],
      allowPlainTextPkce: [null],
      allowAccessTokensViaBrowser: [null],
    });

    // Init form authentication
    this.authenticationForm = this.fb.group({
      enableLocalLogin: [null],
      frontChannelLogoutUri: [null],
      frontChannelLogoutSessionRequired: [null],
      backChannelLogoutUri: [null],
      backChannelLogoutSessionRequired: [null],
      userSsoLifetime: [null]
    });

    // Init form token
    this.tokenForm = this.fb.group({
      identityTokenLifetime: [null],
      accessTokenLifetime: [null],
      accessTokenType: [null],
      authorizationCodeLifetime: [null],
      absoluteRefreshTokenLifetime: [null],
      slidingRefreshTokenLifetime: [null],
      refreshTokenUsage: [null],
      refreshTokenExpiration: [null],
      updateAccessTokenClaimsOnRefresh: [null],
      includeJwtId: [null],
      alwaysSendClientClaims: [null],
      alwaysIncludeUserClaimsInIdToken: [null],
      pairWiseSubjectSalt: [null],
      clientClaimsPrefix: [null],
    });

    // Init form device Flow
    this.deviceFlowForm = this.fb.group({
      userCodeType: [null],
      deviceCodeLifetime: [null]
    });

    // Init form client secrets
    this.validateFormClientSecrets = this.fb.group({
      type: ['SharedSecret'],
      value: [null, Validators.required],
      description: [null],
      expiration: [null],
      hashType: ['Sha256'],
    });

    // Init form client claims
    this.validateFormClientClaims = this.fb.group({
      type: ['SharedSecret'],
      value: [null, Validators.required]
    });

    this.getClientDetail(this.clientId);
    this.getBasicSetting(this.clientId);
    this.getSettingSetting(this.clientId);
    this.getAllScope();
    this.getAuthentication(this.clientId);
    this.getToken(this.clientId);
    this.getDeviceFlow(this.clientId);
    this.getClientSecret(this.clientId);
    this.getClientClaim(this.clientId);
  }

  //#region Client Detail
  // Load client detail
  getClientDetail(clientId: string): void {
    this.isSpinning = true;
    this.clientServices.getDetail(clientId)
      .subscribe((res: any) => {
        this.clientId = res.clientId;
        this.clientName = res.clientName;
        this.clientUri = res.clientUri;
        setTimeout(() => {
          this.isSpinning = false;
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
  //#endregion

  //#region Basic setting

  // Client setting basic
  getBasicSetting(clientId: string) {
    this.isSpinning = true;
    this.clientServices.getBasic(clientId)
      .subscribe((res: any) => {
        this.basicForm.setValue({
          clientId: res.clientId,
          clientName: res.clientName,
          description: res.description,
          clientUri: res.clientUri,
          logoUri: res.logoUri
        });
        this.originTags = res.allowedCorsOrigins;
        setTimeout(() => {
          this.isSpinning = false;
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

  submitBasicForm() {
    const clientName = this.basicForm.get('clientName').value;
    const clientUri = this.basicForm.get('clientUri').value;
    if (clientName === '' || clientUri === '') {
      this.createNotification(
        MessageConstants.TYPE_NOTIFICATION_ERROR,
        MessageConstants.TITLE_NOTIFICATION_SSO,
        'Client name and uri are required!',
        'bottomRight'
      );
    } else {
      this.isSpinning = true;
      this.clientServices.putBasic(this.clientId, this.basicForm.getRawValue())
        .subscribe(() => {
          this.createNotification(
            MessageConstants.TYPE_NOTIFICATION_SUCCESS,
            MessageConstants.TITLE_NOTIFICATION_SSO,
            MessageConstants.NOTIFICATION_UPDATE,
            'bottomRight'
          );
          setTimeout(() => {
            this.ngOnInit();
            this.isSpinning = false;
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
  }

  // Origins
  sliceTagName(tag: string): string {
    const isLongTag = tag.length > 50;
    return isLongTag ? `${tag.slice(0, 50)}...` : tag;
  }

  showInputOrigin(): void {
    this.inputVisibleOrigin = true;
    setTimeout(() => {
      this.inputElement?.nativeElement.focus();
    }, 10);
  }

  handleInputConfirmOrigin(): void {
    if (this.inputValueOrigin && this.originTags.indexOf(this.inputValueOrigin) === -1) {
      this.originTags = [...this.originTags, this.inputValueOrigin];
      this.addBasicOrigin(this.inputValueOrigin);
    }
    this.inputValueOrigin = '';
    this.inputVisibleOrigin = false;
  }
  addBasicOrigin(origin: string): void {
    const data = Object.assign({ origin });
    this.isSpinning = true;
    this.clientServices.postBasicOrigin(this.clientId, data)
      .subscribe(() => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_ADD,
          'bottomRight'
        );
        setTimeout(() => {
          this.isSpinning = false;
        }, 500);
      }, errorMessage => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_ERROR,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          errorMessage,
          'bottomRight'
        );
        setTimeout(() => {
          console.log(this.originTags);
          this.originTags.splice(this.originTags.length - 1, 1);
          console.log(this.originTags);
          this.isSpinning = false;
        }, 500);
      });
  }

  deleteOrigin(removedTag: {}): void {
    this.originTags = this.originTags.filter(tag => tag !== removedTag);
    this.isSpinning = true;
    this.clientServices.deleteBasicOrigin(this.clientId, removedTag)
      .subscribe(() => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_DELETE,
          'bottomRight'
        );
        setTimeout(() => {
          this.isSpinning = false;
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
  //#endregion

  //#region Setting

  // Client setting setting
  getSettingSetting(clientId: string) {
    this.isSpinning = true;
    this.clientServices.getSetting(clientId)
      .subscribe((res: any) => {
        this.settingForm.setValue({
          enabled: res.enabled,
          requireConsent: res.requireConsent,
          allowRememberConsent: res.allowRememberConsent,
          allowOfflineAccess: res.allowOfflineAccess,
          requireClientSecret: res.requireClientSecret,
          protocolType: res.protocolType,
          requirePkce: res.requirePkce,
          allowPlainTextPkce: res.allowPlainTextPkce,
          allowAccessTokensViaBrowser: res.allowAccessTokensViaBrowser,
        });
        this.redirectUriTags = res.redirectUris;
        this.allowedScopes = res.allowedScopes;
        this.allowedGrantTypes = res.allowedGrantTypes;
        setTimeout(() => {
          this.isSpinning = false;
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

  submitSettingForm() {
    this.isSpinning = true;
    this.clientServices.putSetting(this.clientId, this.settingForm.getRawValue())
      .subscribe(() => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_UPDATE,
          'bottomRight'
        );
        setTimeout(() => {
          this.ngOnInit();
          this.isSpinning = false;
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

  // Grant Types
  addClientGrantType(grantType: string) {
    this.isSpinning = true;
    const data = Object.assign({ grantType });
    this.clientServices.postSettingGrantType(this.clientId, data)
      .subscribe(() => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_ADD,
          'bottomRight');
        setTimeout(() => {
          this.ngOnInit();
          this.isSpinning = false;
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

  deleteClientGrantType(removedTag: string) {
    this.originTags = this.originTags.filter(tag => tag !== removedTag);
    this.isSpinning = true;
    this.clientServices.deleteSettingGrantType(this.clientId, removedTag)
      .subscribe(() => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_DELETE,
          'bottomRight'
        );
        setTimeout(() => {
          this.isSpinning = false;
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

  // Scopes
  getAllScope() {
    this.clientServices.getAllScope()
      .subscribe((res: any[]) => {
        this.scopeTags = res;
        console.log(this.scopeTags);
      }, errorMessage => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_ERROR,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          errorMessage,
          'bottomRight'
        );
      });
  }

  addClientScope(scope: string) {
    this.isSpinning = true;
    const data = Object.assign({ scope });
    this.clientServices.postSettingScope(this.clientId, data)
      .subscribe(() => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_ADD,
          'bottomRight');
        setTimeout(() => {
          this.ngOnInit();
          this.isSpinning = false;
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

  deleteClientScope(removedTag: string) {
    this.originTags = this.originTags.filter(tag => tag !== removedTag);
    this.isSpinning = true;
    this.clientServices.deleteSettingScope(this.clientId, removedTag)
      .subscribe(() => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_DELETE,
          'bottomRight'
        );
        setTimeout(() => {
          this.isSpinning = false;
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

  // Redirect Uri
  deleteRedirectUri(removedTag: {}): void {
    this.originTags = this.originTags.filter(tag => tag !== removedTag);
    this.isSpinning = true;
    this.clientServices.deleteSettingRedirectUri(this.clientId, removedTag)
      .subscribe(() => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_DELETE,
          'bottomRight'
        );
        setTimeout(() => {
          this.isSpinning = false;
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


  showInputRedirectUri(): void {
    this.inputVisibleRedirectUri = true;
    setTimeout(() => {
      this.inputElement?.nativeElement.focus();
    }, 10);
  }

  handleInputConfirmRedirectUri(): void {
    if (this.inputValueRedirectUri && this.redirectUriTags.indexOf(this.inputValueRedirectUri) === -1) {
      this.redirectUriTags = [...this.redirectUriTags, this.inputValueRedirectUri];
      this.addSettingRedirectUri(this.inputValueRedirectUri);
    }
    this.inputValueRedirectUri = '';
    this.inputVisibleRedirectUri = false;
  }
  addSettingRedirectUri(redirectUri: string): void {
    const data = Object.assign({ redirectUri });
    this.isSpinning = true;
    this.clientServices.postSettingRedirectUri(this.clientId, data)
      .subscribe(() => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_ADD,
          'bottomRight'
        );
        setTimeout(() => {
          this.isSpinning = false;
        }, 500);
      }, errorMessage => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_ERROR,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          errorMessage,
          'bottomRight'
        );
        setTimeout(() => {
          console.log(this.originTags);
          this.originTags.splice(this.originTags.length - 1, 1);
          console.log(this.originTags);
          this.isSpinning = false;
        }, 500);
      });
  }

  // Drawer
  openClientSecrets(): void {
    this.visibleClientSecrets = true;
  }

  closeClientSecrets(): void {
    this.visibleClientSecrets = false;
  }

  // Get client secret
  getClientSecret(clientId: string) {
    this.isSpinning = true;
    this.clientServices.getClientSecret(clientId)
      .subscribe((res: any[]) => {
        this.itemClientSecrets = res;
        setTimeout(() => {
          this.isSpinning = false;
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


  // Create new client secret
  submitFormClientSecrets(): void {
    this.isSpinning = true;
    const data = this.validateFormClientSecrets.getRawValue();
    this.clientServices.addClientSecret(this.clientId, data)
      .subscribe(() => {
        this.ngOnInit();
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_ADD + name + '!',
          'bottomRight');
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

  // Delete client secret
  deleteClientSecret(id: string) {
    this.isSpinning = true;
    this.clientServices.deleteClientSecret(this.clientId, Number(id))
      .subscribe(() => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_DELETE + name + ' !', 'bottomRight');
        this.ngOnInit();
        setTimeout(() => {
          this.isSpinning = false;
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

  // Delete client secret
  showDeleteConfirmClientSecrets(id: string): void {
    this.confirmDeleteClientSecret = this.modal.confirm({
      nzTitle: 'Do you Want to delete client secrets' + id + ' ?',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzOnOk: () =>
        new Promise((resolve, reject) => {
          this.deleteClientSecret(id);
          setTimeout(Math.random() > 0.5 ? resolve : reject, 200);
        })
    });
  }

  //#endregion

  //#region Authentication setting

  // Client setting authentication
  getAuthentication(clientId: string) {
    this.isSpinning = true;
    this.clientServices.getAuthentication(clientId)
      .subscribe((res: any) => {
        this.authenticationForm.setValue({
          enableLocalLogin: res.enableLocalLogin,
          frontChannelLogoutUri: res.frontChannelLogoutUri,
          frontChannelLogoutSessionRequired: res.frontChannelLogoutSessionRequired,
          backChannelLogoutUri: res.backChannelLogoutUri,
          backChannelLogoutSessionRequired: res.backChannelLogoutSessionRequired,
          userSsoLifetime: res.userSsoLifetime,
        });
        this.postLogoutRedirectUris = res.postLogoutRedirectUris;
        setTimeout(() => {
          this.isSpinning = false;
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
  submitAuthentication() {
    this.isSpinning = true;
    this.clientServices.putAuthentication(this.clientId, this.authenticationForm.getRawValue())
      .subscribe(() => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_UPDATE,
          'bottomRight'
        );
        setTimeout(() => {
          this.ngOnInit();
          this.isSpinning = false;
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

  // Post logout redirect uri
  showInputLogoutUri(): void {
    this.inputVisibleLogoutUri = true;
    setTimeout(() => {
      this.inputElement?.nativeElement.focus();
    }, 10);
  }

  handleInputConfirmLogoutUri(): void {
    if (this.inputValueLogoutUri && this.postLogoutRedirectUris.indexOf(this.inputValueLogoutUri) === -1) {
      this.postLogoutRedirectUris = [...this.postLogoutRedirectUris, this.inputValueLogoutUri];
      this.addAuthenticationLogoutUri(this.inputValueLogoutUri);
    }
    this.inputValueLogoutUri = '';
    this.inputVisibleLogoutUri = false;
  }
  addAuthenticationLogoutUri(postLogoutRedirectUri: string): void {
    const data = Object.assign({ postLogoutRedirectUri });
    this.isSpinning = true;
    this.clientServices.postPostLogoutRedirectUris(this.clientId, data)
      .subscribe(() => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_ADD,
          'bottomRight'
        );
        setTimeout(() => {
          this.isSpinning = false;
        }, 500);
      }, errorMessage => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_ERROR,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          errorMessage,
          'bottomRight'
        );
        setTimeout(() => {
          console.log(this.originTags);
          this.originTags.splice(this.originTags.length - 1, 1);
          console.log(this.originTags);
          this.isSpinning = false;
        }, 500);
      });
  }

  deletePostLogoutRedirectUris(removedTag: string) {
    this.originTags = this.originTags.filter(tag => tag !== removedTag);
    this.isSpinning = true;
    this.clientServices.deletePostLogoutRedirectUris(this.clientId, removedTag)
      .subscribe(() => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_DELETE,
          'bottomRight'
        );
        setTimeout(() => {
          this.isSpinning = false;
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

  //#endregion

  //#region Token setting

  // Token
  getToken(clientId: string) {
    this.isSpinning = true;
    this.clientServices.getToken(clientId)
      .subscribe((res: any) => {
        this.tokenForm.setValue({
          identityTokenLifetime: res.identityTokenLifetime,
          accessTokenLifetime: res.accessTokenLifetime,
          accessTokenType: res.accessTokenType,
          authorizationCodeLifetime: res.authorizationCodeLifetime,
          absoluteRefreshTokenLifetime: res.absoluteRefreshTokenLifetime,
          slidingRefreshTokenLifetime: res.slidingRefreshTokenLifetime,
          refreshTokenUsage: res.refreshTokenUsage,
          refreshTokenExpiration: res.refreshTokenExpiration,
          updateAccessTokenClaimsOnRefresh: res.updateAccessTokenClaimsOnRefresh,
          includeJwtId: res.includeJwtId,
          alwaysSendClientClaims: res.alwaysSendClientClaims,
          alwaysIncludeUserClaimsInIdToken: res.alwaysIncludeUserClaimsInIdToken,
          pairWiseSubjectSalt: res.pairWiseSubjectSalt,
          clientClaimsPrefix: res.clientClaimsPrefix
        });
        setTimeout(() => {
          this.isSpinning = false;
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

  submitToken() {
    this.isSpinning = true;
    this.clientServices.putToken(this.clientId, this.tokenForm.getRawValue())
      .subscribe(() => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_UPDATE,
          'bottomRight'
        );
        setTimeout(() => {
          this.ngOnInit();
          this.isSpinning = false;
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

  // Client claim

  // Drawer
  openClientClaims(): void {
    this.visibleClientClaims = true;
  }

  closeClientClaims(): void {
    this.visibleClientClaims = false;
  }

  // Get client claim
  getClientClaim(clientId: string) {
    this.isSpinning = true;
    this.clientServices.getClientClaims(clientId)
      .subscribe((res: any[]) => {
        this.itemClientClaims = res;
        setTimeout(() => {
          this.isSpinning = false;
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


  // Create new client claim
  submitFormClientClaims(): void {
    this.isSpinning = true;
    const data = this.validateFormClientClaims.getRawValue();
    this.clientServices.addClientClaim(this.clientId, data)
      .subscribe(() => {
        this.ngOnInit();
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_ADD + name + '!',
          'bottomRight');
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

  // Delete client claim
  deleteClientClaim(id: string) {
    this.isSpinning = true;
    this.clientServices.deleteClientClaim(this.clientId, Number(id))
      .subscribe(() => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_DELETE, 'bottomRight');
        this.ngOnInit();
        setTimeout(() => {
          this.isSpinning = false;
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

  // Delete client claim
  showDeleteConfirmClientClaims(id: string): void {
    this.confirmDeleteClientClaim = this.modal.confirm({
      nzTitle: 'Do you Want to delete client claims' + id + ' ?',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzOnOk: () =>
        new Promise((resolve, reject) => {
          this.deleteClientClaim(id);
          setTimeout(Math.random() > 0.5 ? resolve : reject, 200);
        })
    });
  }

  //#endregion

  //#region Device Flow setting

  // Device Flow
  getDeviceFlow(clientId: string) {
    this.isSpinning = true;
    this.clientServices.getDeviceFlow(clientId)
      .subscribe((res: any) => {
        this.deviceFlowForm.setValue({
          userCodeType: res.userCodeType,
          deviceCodeLifetime: res.deviceCodeLifetime
        });
        setTimeout(() => {
          this.isSpinning = false;
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
  submitDeviceFlow() {
    this.isSpinning = true;
    this.clientServices.putDeviceFlow(this.clientId, this.tokenForm.getRawValue())
      .subscribe(() => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_UPDATE,
          'bottomRight'
        );
        setTimeout(() => {
          this.ngOnInit();
          this.isSpinning = false;
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

  //#endregion

  // notification
  createNotification(type: string, title: string, content: string, position: NzNotificationPlacement): void {
    this.notification.create(type, title, content, { nzPlacement: position });
  }

}
