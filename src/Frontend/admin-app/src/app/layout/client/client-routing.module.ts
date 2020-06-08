import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes=[
    {path: '', loadChildren: ()=>import('./list-client/list-client.module').then(m=>m.ListClientModule)},
    {path: 'add', loadChildren: ()=>import('./add-client/add-client.module').then(m=>m.AddClientModule)},
    {path: 'edit', loadChildren: ()=>import('./edit-client/edit-client.module').then(m=>m.EditClientModule)}
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ClientRoutingModule{}