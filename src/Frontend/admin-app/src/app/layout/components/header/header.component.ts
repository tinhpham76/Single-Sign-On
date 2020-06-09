import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { AuthService } from '@app/shared/services/auth.service';
import { UserService } from '@app/shared/services/users.services';

@Component({
    selector: 'app-header',
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
    public pushRightClass: string;
    fullName: string;
    userName: string;
    isAuthenticated: boolean;
    subscription: Subscription;
    email: string;
    role: string;

    constructor(private translate: TranslateService, public router: Router,
        private authService: AuthService, private userService: UserService) {
        this.loadProfile();

        this.subscription = this.authService.authNavStatus$.subscribe(status => this.isAuthenticated = status);
        this.userName = this.authService.name;

        this.router.events.subscribe(val => {
            if (
                val instanceof NavigationEnd &&
                window.innerWidth <= 992 &&
                this.isToggled()
            ) {
                this.toggleSidebar();
            }
        });
    }

    ngOnInit() {
        this.pushRightClass = 'push-right';
    }

    isToggled(): boolean {
        const dom: Element = document.querySelector('body');
        return dom.classList.contains(this.pushRightClass);
    }

    toggleSidebar() {
        const dom: any = document.querySelector('body');
        dom.classList.toggle(this.pushRightClass);
    }

    rltAndLtr() {
        const dom: any = document.querySelector('body');
        dom.classList.toggle('rtl');
    }

    async signout() {
        await this.authService.signout();
    }

    changeLang(language: string) {
        this.translate.use(language);
    }
    loadProfile() {
        const profile = this.authService.profile;
        this.fullName = profile.fullName;
        this.email = profile.email;
        this.role = profile.role;        
    }
}
