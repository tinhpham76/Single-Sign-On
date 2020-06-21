import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IdentityResourceComponent } from './identity-resource.component';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { NzDescriptionsModule } from 'ng-zorro-antd/descriptions';
import { NzNotificationModule } from 'ng-zorro-antd/notification';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NzDrawerModule } from 'ng-zorro-antd/drawer';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzAlertModule } from 'ng-zorro-antd/alert';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { IdentityResourceRoutingModule } from './identity-resource-routing.module';
import { AddIdentityResourceComponent } from './add-identity-resource/add-identity-resource.component';
import { NzSwitchModule } from 'ng-zorro-antd/switch';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { IdentityClaimComponent } from './identity-claim/identity-claim.component';


@NgModule({
  declarations: [IdentityResourceComponent, AddIdentityResourceComponent, IdentityClaimComponent],
  imports: [
    CommonModule,
    NzTableModule,
    NzDividerModule,
    NzSpinModule,
    NzSwitchModule,
    NzDescriptionsModule,
    NzNotificationModule,
    NzCheckboxModule,
    NzButtonModule,
    NzModalModule,
    NzTagModule,
    NzToolTipModule,
    NzIconModule,
    FormsModule,
    ReactiveFormsModule,
    NzDrawerModule,
    NzGridModule,
    NzDatePickerModule,
    NzLayoutModule,
    NzFormModule,
    NzAlertModule,
    NzInputModule,
    NzModalModule,
    NzSelectModule,
    IdentityResourceRoutingModule
  ]
})
export class IdentityResourceModule { }
