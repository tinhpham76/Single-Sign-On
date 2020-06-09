import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ListClientComponent } from './list-client/list-client.component';
import { AddClientComponent } from './add-client/add-client.component';
import { EditClientComponent } from './edit-client/edit-client.component';

const routes: Routes=[
    {path: '', component: ListClientComponent},
    {path: 'add', component: AddClientComponent},
    {path: 'edit', component: EditClientComponent}
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ClientRoutingModule{}