import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EditIdentityResourceComponent } from './edit-identity-resource.component';
import { EditIdentityResourceRoutingModule } from './edit-identity-resource-routing.module';



@NgModule({
  declarations: [EditIdentityResourceComponent],
  imports: [
    CommonModule,
    EditIdentityResourceRoutingModule
  ]
})
export class EditIdentityResourceModule { }
