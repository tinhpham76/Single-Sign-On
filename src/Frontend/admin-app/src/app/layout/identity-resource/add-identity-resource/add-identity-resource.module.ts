import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddIdentityResourceComponent } from './add-identity-resource.component';
import { AddIdentityResourceRoutingModule } from './add-identity-resource-routing.module';



@NgModule({
  declarations: [AddIdentityResourceComponent],
  imports: [
    CommonModule,
    AddIdentityResourceRoutingModule
  ]
})
export class AddIdentityResourceModule { }
