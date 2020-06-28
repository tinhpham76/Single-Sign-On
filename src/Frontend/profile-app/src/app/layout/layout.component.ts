import { Component, OnInit } from '@angular/core';
import { AuthService } from '@app/shared/services/auth.service';
import { Subscription } from 'rxjs';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { MessageConstants } from '@app/shared/constants/messages.constant';

@Component({
    selector: 'app-layout',
    templateUrl: './layout.component.html',
    styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit {

    subscription: Subscription;
    userName: string;
    isAuthenticated: boolean;
    fullName: string;
    email: string;
    constructor(
        private authServices: AuthService,
        private notification: NzNotificationService
    ) {
        this.subscription = this.authServices.authNavStatus$.subscribe(status => this.isAuthenticated = status);
        this.userName = this.authServices.name;
        const profile = this.authServices.profile;
        this.fullName = profile.fullName;
        this.email = profile.email;
        this.createNotification(this.authServices.name);
    }

    ngOnInit() { }
    async signOut() {
        await this.authServices.signOut();
    }
    createNotification(content: string): void {
        this.notification.create(
            MessageConstants.TYPE_NOTIFICATION_SUCCESS,
            MessageConstants.TITLE_NOTIFICATION_SSO,
            MessageConstants.NOTIFICATION_WELCOME + content
        );
    }

}
