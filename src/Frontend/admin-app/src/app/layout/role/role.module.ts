import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RoleComponent } from './role.component';
import { RoleRoutingModule } from './role-routing.module';
import {PanelModule} from 'primeng/panel';
import {ButtonModule} from 'primeng/button';
import {TableModule} from 'primeng/table';
import {PaginatorModule} from 'primeng/paginator'
import {BlockUIModule} from 'primeng/blockui';
import {ProgressSpinnerModule} from 'primeng/progressspinner';
import { from } from 'rxjs';


@NgModule({
  declarations: [RoleComponent],
  imports: [
    CommonModule,
    PanelModule,
    ButtonModule,
    TableModule,
    PaginatorModule,
    BlockUIModule,
    ProgressSpinnerModule,
    RoleRoutingModule
  ]
})
export class RoleModule { }
