import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IdentityResourceComponent } from './identity-resource.component';
import { IdentityResourceRoutingModule } from './identity-resource-routin.module';



@NgModule({
  declarations: [IdentityResourceComponent],
  imports: [
    CommonModule,
    IdentityResourceRoutingModule
  ]
})
export class IdentityResourceModule { }
