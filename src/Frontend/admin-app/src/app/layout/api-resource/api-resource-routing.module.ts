import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ApiResourceComponent } from './api-resource.component';

const routes: Routes=[
    {path: '', loadChildren: ()=>import('./list-api-resource/list-api-resource.module').then(m=>m.ListApiResourceModule)},
    {path: 'add', loadChildren: ()=>import('./add-api-resource/add-api-resource.module').then(m=>m.AddApiResourceModule)},
    {path: 'edit', loadChildren: ()=>import('./edit-api-resource/edit-api-resource.module').then(m=>m.EditApiResourceModule)}
]

@NgModule({
    imports:[RouterModule.forChild(routes)],
    exports:[RouterModule]
})
export class ApiResourceRoutingModule{}