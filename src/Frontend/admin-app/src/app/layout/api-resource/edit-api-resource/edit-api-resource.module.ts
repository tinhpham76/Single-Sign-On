import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EditApiResourceComponent } from './edit-api-resource.component';
import { EditApiResourceRoutingModule } from './edit-api-resource-routing.module';



@NgModule({
  declarations: [EditApiResourceComponent],
  imports: [
    CommonModule,
    EditApiResourceRoutingModule
  ]
})
export class EditApiResourceModule { }
