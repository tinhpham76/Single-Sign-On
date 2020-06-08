import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiResourceRoutingModule } from './api-resource-routing.module';
import { ApiResourceComponent } from './api-resource.component';



@NgModule({
  declarations: [ApiResourceComponent],
  imports: [
    CommonModule,
    ApiResourceRoutingModule
  ]
})
export class ApiResourceModule { }
