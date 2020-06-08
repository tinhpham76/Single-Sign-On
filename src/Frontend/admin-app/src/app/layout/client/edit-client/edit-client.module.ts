import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EditClientRoutingModule } from './edit-client-routing.module';
import { EditClientComponent } from './edit-client.component';



@NgModule({
  declarations: [EditClientComponent],
  imports: [
    CommonModule,
    EditClientRoutingModule
  ]
})
export class EditClientModule { }
