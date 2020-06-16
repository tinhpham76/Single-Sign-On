import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserComponent } from './user.component';
import { UserRoutingModule } from './user-routing.module';
import { NzTableModule } from 'ng-zorro-antd/table';


@NgModule({
  declarations: [UserComponent],
  imports: [
    CommonModule,
    NzTableModule,
    UserRoutingModule
  ]
})
export class UserModule { }
