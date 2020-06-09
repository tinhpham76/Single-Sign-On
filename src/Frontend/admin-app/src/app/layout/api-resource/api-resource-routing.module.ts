import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ApiResourceComponent } from './api-resource.component';
import { ListApiResourceComponent } from './list-api-resource/list-api-resource.component';
import { AddApiResourceComponent } from './add-api-resource/add-api-resource.component';
import { EditClientComponent } from '../client/edit-client/edit-client.component';

const routes: Routes=[
    {path: '', component: ListApiResourceComponent},
    {path: 'add',component: AddApiResourceComponent},
    {path: 'edit',component: EditClientComponent}
]

@NgModule({
    imports:[RouterModule.forChild(routes)],
    exports:[RouterModule]
})
export class ApiResourceRoutingModule{}