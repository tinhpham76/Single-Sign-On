import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiResourceRoutingModule } from './api-resource-routing.module';
import { ApiResourceComponent } from './api-resource.component';
import { EditClientComponent } from '../client/edit-client/edit-client.component';
import { AddApiResourceComponent } from './add-api-resource/add-api-resource.component';
import { ListApiResourceComponent } from './list-api-resource/list-api-resource.component';
import { TranslateModule } from '@ngx-translate/core';



@NgModule({
  declarations: [ApiResourceComponent, ListApiResourceComponent, AddApiResourceComponent, EditClientComponent],
  imports: [
    CommonModule,
    TranslateModule,
    ApiResourceRoutingModule
  ]
})
export class ApiResourceModule { }
