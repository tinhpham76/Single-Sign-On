import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ListApiResourceComponent } from './list-api-resource.component';
import { ListApiResourceRoutingModule } from './list-api-resource-routing.module';



@NgModule({
  declarations: [ListApiResourceComponent],
  imports: [
    CommonModule,
    ListApiResourceRoutingModule
  ]
})
export class ListApiResourceModule { }
