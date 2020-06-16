import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';

import { LoginRoutingModule } from './login-routing.module';
import { LoginComponent } from './login.component';
import { NgxSpinnerService } from 'ngx-spinner';

import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzAlertModule } from 'ng-zorro-antd/alert';
import { NzNotificationModule } from 'ng-zorro-antd/notification';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
    imports: [
        CommonModule,
        TranslateModule,
        NzLayoutModule,
        NzInputModule,
        NzAlertModule,
        NzNotificationModule,
        LoginRoutingModule,
    ],
    declarations: [LoginComponent],
    providers:[NgxSpinnerService]
})
export class LoginModule {}
