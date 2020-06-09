import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ClientComponent } from './client.component';
import { ClientRoutingModule } from './client-routing.module';
import { TranslateModule } from '@ngx-translate/core';
import { ListClientComponent } from './list-client/list-client.component';
import { EditClientComponent } from './edit-client/edit-client.component';
import { AddClientComponent } from './add-client/add-client.component';


@NgModule({
  declarations: [ClientComponent, ListClientComponent, AddClientComponent, EditClientComponent],
  imports: [
    CommonModule,
    TranslateModule,
    ClientRoutingModule
  ]
})
export class ClientModule { }
