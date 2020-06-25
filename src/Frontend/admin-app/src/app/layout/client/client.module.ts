import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ClientComponent } from './client.component';
import { ClientRoutingModule } from './client-routing.module';
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
import { AddClientComponent } from './add-client/add-client.component';
import { EditClientComponent } from './edit-client/edit-client.component';
import { NzTabsModule } from 'ng-zorro-antd/tabs';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzSwitchModule } from 'ng-zorro-antd/switch';

@NgModule({
  declarations: [ClientComponent, AddClientComponent, EditClientComponent],
  imports: [
    CommonModule,
    NzTableModule,
    NzTabsModule,
    NzButtonModule,
    NzDividerModule,
    NzSwitchModule,
    NzSpinModule,
    NzDescriptionsModule,
    NzNotificationModule,
    NzCardModule,
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
    NzFormModule,
    NzAlertModule,
    NzInputModule,
    NzModalModule,
    NzSelectModule,
    ClientRoutingModule
  ]
})
export class ClientModule { }
