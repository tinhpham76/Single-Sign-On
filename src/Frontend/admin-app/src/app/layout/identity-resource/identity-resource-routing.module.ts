import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ListIdentityResourceComponent } from './list-identity-resource/list-identity-resource.component';
import { AddIdentityResourceComponent } from './add-identity-resource/add-identity-resource.component';
import { EditIdentityResourceComponent } from './edit-identity-resource/edit-identity-resource.component';

const routes: Routes=[
    {path: '', component: ListIdentityResourceComponent},
    {path: 'add', component: AddIdentityResourceComponent},
    {path: 'edit', component: EditIdentityResourceComponent}
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class IdentityResourceRoutingModule{}