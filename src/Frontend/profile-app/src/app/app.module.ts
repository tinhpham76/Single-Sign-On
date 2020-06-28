import { CommonModule, DatePipe } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LanguageTranslationModule } from './shared/modules/language-translation/language-translation.module'

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthInterceptor } from './shared/interceptors/auth.interceptor';
import { AuthGuard } from './shared/guard/auth.guard';
import { ToastrModule } from 'ngx-toastr';
import { ServerErrorComponent } from './server-error/server-error.component';
import { AccessDeniedComponent } from './access-denied/access-denied.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { TranslateModule } from '@ngx-translate/core';

import { NZ_I18N } from 'ng-zorro-antd/i18n';
import { en_US } from 'ng-zorro-antd/i18n';
import { registerLocaleData } from '@angular/common';
import en from '@angular/common/locales/en';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzNotificationModule } from 'ng-zorro-antd/notification';
import { NzResultModule } from 'ng-zorro-antd/result';
import { NzButtonModule } from 'ng-zorro-antd/button';

registerLocaleData(en);
@NgModule({
    imports: [
        CommonModule,
        BrowserModule,
        BrowserAnimationsModule,
        HttpClientModule,
        NzNotificationModule,
        LanguageTranslationModule,
        TranslateModule,
        NzIconModule,
        NzResultModule,
        NzButtonModule,
        AppRoutingModule
    ],
    declarations: [AppComponent, ServerErrorComponent, AccessDeniedComponent, NotFoundComponent],
    providers: [AuthGuard, DatePipe,
        {
            provide: HTTP_INTERCEPTORS,
            useClass: AuthInterceptor,
            multi: true
        },
        { provide: NZ_I18N, useValue: en_US }
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
