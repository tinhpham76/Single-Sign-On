import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IdentityResourceComponent } from './identity-resource.component';
import { IdentityResourceRoutingModule } from './identity-resource-routing.module';
import { ListIdentityResourceComponent } from './list-identity-resource/list-identity-resource.component';
import { AddIdentityResourceComponent } from './add-identity-resource/add-identity-resource.component';
import { EditIdentityResourceComponent } from './edit-identity-resource/edit-identity-resource.component';
import { TranslateModule } from '@ngx-translate/core';




@NgModule({
  declarations: [IdentityResourceComponent, ListIdentityResourceComponent, AddIdentityResourceComponent, EditIdentityResourceComponent],
  imports: [
    CommonModule,
    TranslateModule,
    IdentityResourceRoutingModule
  ]
})
export class IdentityResourceModule { }
