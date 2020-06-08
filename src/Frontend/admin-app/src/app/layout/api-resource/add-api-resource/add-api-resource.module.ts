import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddApiResourceComponent } from './add-api-resource.component';
import { AddApiResourceRoutingModule } from './add-api-resource-routing.module';



@NgModule({
  declarations: [AddApiResourceComponent],
  imports: [
    CommonModule,
    AddApiResourceRoutingModule
  ]
})
export class AddApiResourceModule { }
