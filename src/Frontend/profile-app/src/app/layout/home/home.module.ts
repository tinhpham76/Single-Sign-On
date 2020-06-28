import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeRoutingModule } from './home-routing.module';
import { HomeComponent } from './home.component';
import { TranslateModule } from '@ngx-translate/core';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';

@NgModule({
  declarations: [HomeComponent],
  imports: [
    CommonModule,
    TranslateModule,
    NzButtonModule,
    NzGridModule,
    NzIconModule,
    NzToolTipModule,
    NzCardModule,
    HomeRoutingModule
  ]
})
export class HomeModule { }
