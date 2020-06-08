import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';

import { LayoutRoutingModule } from './layout-routing.module';
import { LayoutComponent } from './layout.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { HeaderComponent } from './components/header/header.component';
import { HomeComponent } from './home/home.component';
import { ClientComponent } from './client/client.component';
import { ApiResourceComponent } from './api-resource/api-resource.component';
import { IdentityResourceComponent } from './identity-resource/identity-resource.component';
import { UserComponent } from './user/user.component';
import { RoleComponent } from './role/role.component';

@NgModule({
    imports: [
        CommonModule,
        LayoutRoutingModule,
        TranslateModule,
        NgbDropdownModule
    ],
    declarations: [LayoutComponent, SidebarComponent, HeaderComponent]
})
export class LayoutModule {}
