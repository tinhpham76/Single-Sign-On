import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddIdentityResourceComponent } from './add-identity-resource.component';

const routes: Routes=[
    {path:'', component: AddIdentityResourceComponent}
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AddIdentityResourceRoutingModule{}