import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddClientComponent } from './add-client.component';
import { AddClientRoutingModule } from './add-client-routing.module';



@NgModule({
  declarations: [AddClientComponent],
  imports: [
    CommonModule,
    AddClientRoutingModule
  ]
})
export class AddClientModule { }
