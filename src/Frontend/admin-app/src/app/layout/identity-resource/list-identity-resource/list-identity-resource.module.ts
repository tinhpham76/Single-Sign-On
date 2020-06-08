import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ListIdentityResourceComponent } from './list-identity-resource.component';
import { ListIdentityResourcceRoutingModule } from './list-identity-resource-routing.module';



@NgModule({
  declarations: [ListIdentityResourceComponent],
  imports: [
    CommonModule,
    ListIdentityResourcceRoutingModule
  ]
})
export class ListIdentityResourceModule { }
