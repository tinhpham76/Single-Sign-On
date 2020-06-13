import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiResourceComponent } from './api-resource.component';
import { ApiResourceRoutingModule } from './api-resource-routing.module';



@NgModule({
  declarations: [ApiResourceComponent],
  imports: [
    CommonModule,
    ApiResourceRoutingModule
  ]
})
export class ApiResourceModule { }
