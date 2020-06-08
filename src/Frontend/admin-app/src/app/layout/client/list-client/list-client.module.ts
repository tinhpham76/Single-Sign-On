import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ListClientComponent } from './list-client.component';
import { ListClientRoutingModule } from './list-client-routing.module';



@NgModule({
  declarations: [ListClientComponent],
  imports: [
    CommonModule,
    ListClientRoutingModule
  ]
})
export class ListClientModule { }
