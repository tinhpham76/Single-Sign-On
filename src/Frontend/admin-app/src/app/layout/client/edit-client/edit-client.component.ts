import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { NzNotificationService, NzNotificationPlacement } from 'ng-zorro-antd/notification';
import { ActivatedRoute } from '@angular/router';
import { ClientServices } from '@app/shared/services/clients.service';
import { MessageConstants } from '@app/shared/constants/messages.constant';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { FormGroup, FormBuilder, Validators, Form } from '@angular/forms';
import { isNumber } from 'util';

@Component({
  selector: 'app-edit-client',
  templateUrl: './edit-client.component.html',
  styleUrls: ['./edit-client.component.scss']
})
export class EditClientComponent implements OnInit {

  // Spin
  public isSpinning: boolean;
  public isSpinningBasic: boolean;

  // Client
  public clientId: string;
  public clientName: string;
  public clientUri: string;

  // Form
  public basicForm!: FormGroup;
  public settingForm!: FormGroup;
  public authenticationForm!: FormGroup;

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
  public grantTypes = ['empty', 'web_app_authorization_code', 'web_app_hybrid', 'spa', 'native', 'server', 'device'];

  // Tags Post Logout Redirect Uris
  public postLogoutRedirectUris = [];
  inputVisibleLogoutUri = false;
  inputValueLogoutUri = '';

  @ViewChild('inputElement', { static: false }) inputElement?: ElementRef;

  constructor(private notification: NzNotificationService,
    private route: ActivatedRoute,
    private clientServices: ClientServices,
    private fb: FormBuilder) { }

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
    this.getClientDetail(this.clientId);
    this.getBasicSetting(this.clientId);
    this.getSettingSetting(this.clientId);
    this.getAllScope();
    this.getAuthentication(this.clientId);
  }

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

  // Client setting basic
  getBasicSetting(clientId: string) {
    this.isSpinningBasic = true;
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
          this.isSpinningBasic = false;
        }, 500);
      }, errorMessage => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_ERROR,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          errorMessage,
          'bottomRight'
        );
        setTimeout(() => {
          this.isSpinningBasic = false;
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
      this.isSpinningBasic = true;
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
            this.isSpinningBasic = false;
          }, 500);
        }, errorMessage => {
          this.createNotification(
            MessageConstants.TYPE_NOTIFICATION_ERROR,
            MessageConstants.TITLE_NOTIFICATION_SSO,
            errorMessage,
            'bottomRight'
          );
          setTimeout(() => {
            this.isSpinningBasic = false;
          }, 500);
        });
    }
  }

  // Origins
  deleteOrigin(removedTag: {}): void {
    this.originTags = this.originTags.filter(tag => tag !== removedTag);
    this.isSpinningBasic = true;
    this.clientServices.deleteBasicOrigin(this.clientId, removedTag)
      .subscribe(() => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_DELETE,
          'bottomRight'
        );
        setTimeout(() => {
          this.isSpinningBasic = false;
        }, 500);
      }, errorMessage => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_ERROR,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          errorMessage,
          'bottomRight'
        );
        setTimeout(() => {
          this.isSpinningBasic = false;
        }, 500);
      });
  }

  sliceTagName(tag: string): string {
    const isLongTag = tag.length > 20;
    return isLongTag ? `${tag.slice(0, 20)}...` : tag;
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
    this.isSpinningBasic = true;
    this.clientServices.postBasicOrigin(this.clientId, data)
      .subscribe(() => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_ADD,
          'bottomRight'
        );
        setTimeout(() => {
          this.isSpinningBasic = false;
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
          this.isSpinningBasic = false;
        }, 500);
      });
  }

  ///////////////// Client setting setting
  getSettingSetting(clientId: string) {
    this.isSpinningBasic = true;
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
          this.isSpinningBasic = false;
        }, 500);
      }, errorMessage => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_ERROR,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          errorMessage,
          'bottomRight'
        );
        setTimeout(() => {
          this.isSpinningBasic = false;
        }, 500);
      });
  }
  submitSettingForm() {
    this.isSpinningBasic = true;
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
          this.isSpinningBasic = false;
        }, 500);
      }, errorMessage => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_ERROR,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          errorMessage,
          'bottomRight'
        );
        setTimeout(() => {
          this.isSpinningBasic = false;
        }, 500);
      });
  }

  // Grant Types
  addClientGrantType(grantType: string) {
    this.isSpinningBasic = true;
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
          this.isSpinningBasic = false;
        }, 500);
      }, errorMessage => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_ERROR,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          errorMessage,
          'bottomRight'
        );
        setTimeout(() => {
          this.isSpinningBasic = false;
        }, 500);
      });
  }

  deleteClientGrantType(removedTag: string) {
    this.originTags = this.originTags.filter(tag => tag !== removedTag);
    this.isSpinningBasic = true;
    this.clientServices.deleteSettingGrantType(this.clientId, removedTag)
      .subscribe(() => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_DELETE,
          'bottomRight'
        );
        setTimeout(() => {
          this.isSpinningBasic = false;
        }, 500);
      }, errorMessage => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_ERROR,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          errorMessage,
          'bottomRight'
        );
        setTimeout(() => {
          this.isSpinningBasic = false;
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
    this.isSpinningBasic = true;
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
          this.isSpinningBasic = false;
        }, 500);
      }, errorMessage => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_ERROR,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          errorMessage,
          'bottomRight'
        );
        setTimeout(() => {
          this.isSpinningBasic = false;
        }, 500);
      });
  }

  deleteClientScope(removedTag: string) {
    this.originTags = this.originTags.filter(tag => tag !== removedTag);
    this.isSpinningBasic = true;
    this.clientServices.deleteSettingScope(this.clientId, removedTag)
      .subscribe(() => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_DELETE,
          'bottomRight'
        );
        setTimeout(() => {
          this.isSpinningBasic = false;
        }, 500);
      }, errorMessage => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_ERROR,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          errorMessage,
          'bottomRight'
        );
        setTimeout(() => {
          this.isSpinningBasic = false;
        }, 500);
      });
  }

  // Redirect Uri
  deleteRedirectUri(removedTag: {}): void {
    this.originTags = this.originTags.filter(tag => tag !== removedTag);
    this.isSpinningBasic = true;
    this.clientServices.deleteSettingRedirectUri(this.clientId, removedTag)
      .subscribe(() => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_DELETE,
          'bottomRight'
        );
        setTimeout(() => {
          this.isSpinningBasic = false;
        }, 500);
      }, errorMessage => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_ERROR,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          errorMessage,
          'bottomRight'
        );
        setTimeout(() => {
          this.isSpinningBasic = false;
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
    this.isSpinningBasic = true;
    this.clientServices.postSettingRedirectUri(this.clientId, data)
      .subscribe(() => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_ADD,
          'bottomRight'
        );
        setTimeout(() => {
          this.isSpinningBasic = false;
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
          this.isSpinningBasic = false;
        }, 500);
      });
  }

  ///////////////// Client setting authentication
  getAuthentication(clientId: string) {
    this.isSpinningBasic = true;
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
          this.isSpinningBasic = false;
        }, 500);
      }, errorMessage => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_ERROR,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          errorMessage,
          'bottomRight'
        );
        setTimeout(() => {
          this.isSpinningBasic = false;
        }, 500);
      });
  }
  submitAuthentication() {
    this.isSpinningBasic = true;
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
          this.isSpinningBasic = false;
        }, 500);
      }, errorMessage => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_ERROR,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          errorMessage,
          'bottomRight'
        );
        setTimeout(() => {
          this.isSpinningBasic = false;
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
    this.isSpinningBasic = true;
    this.clientServices.postPostLogoutRedirectUris(this.clientId, data)
      .subscribe(() => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_ADD,
          'bottomRight'
        );
        setTimeout(() => {
          this.isSpinningBasic = false;
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
          this.isSpinningBasic = false;
        }, 500);
      });
  }

  deletePostLogoutRedirectUris(removedTag: string) {
    this.originTags = this.originTags.filter(tag => tag !== removedTag);
    this.isSpinningBasic = true;
    this.clientServices.deletePostLogoutRedirectUris(this.clientId, removedTag)
      .subscribe(() => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_SUCCESS,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          MessageConstants.NOTIFICATION_DELETE,
          'bottomRight'
        );
        setTimeout(() => {
          this.isSpinningBasic = false;
        }, 500);
      }, errorMessage => {
        this.createNotification(
          MessageConstants.TYPE_NOTIFICATION_ERROR,
          MessageConstants.TITLE_NOTIFICATION_SSO,
          errorMessage,
          'bottomRight'
        );
        setTimeout(() => {
          this.isSpinningBasic = false;
        }, 500);
      });
  }


  // notification
  createNotification(type: string, title: string, content: string, position: NzNotificationPlacement): void {
    this.notification.create(type, title, content, { nzPlacement: position });
  }
}
