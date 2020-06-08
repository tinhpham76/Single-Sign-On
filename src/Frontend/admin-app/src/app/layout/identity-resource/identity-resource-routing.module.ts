import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes=[
    {path: '', loadChildren: ()=>import('./list-identity-resource/list-identity-resource.module').then(m=>m.ListIdentityResourceModule)},
    {path: 'add', loadChildren: ()=>import('./add-identity-resource/add-identity-resource.module').then(m=>m.AddIdentityResourceModule)},
    {path: 'edit', loadChildren: ()=>import('./edit-identity-resource/edit-identity-resource.module').then(m=>m.EditIdentityResourceModule)}
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class IdentityResourceRoutingModule{}