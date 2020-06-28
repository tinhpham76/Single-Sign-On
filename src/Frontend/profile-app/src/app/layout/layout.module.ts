import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LayoutComponent } from './layout.component';
import { LayoutRoutingModule } from './layout-routing.module';

import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzNotificationModule } from 'ng-zorro-antd/notification';

@NgModule({
  declarations: [LayoutComponent],
  imports: [
    CommonModule,
    NzLayoutModule,
    NzMenuModule,
    NzToolTipModule,
    FormsModule,
    HttpClientModule,
    NzIconModule,
    NzBreadCrumbModule,
    NzGridModule,
    NzNotificationModule,
    LayoutRoutingModule
  ],
  bootstrap: [LayoutComponent]
})
export class LayoutModule { }
