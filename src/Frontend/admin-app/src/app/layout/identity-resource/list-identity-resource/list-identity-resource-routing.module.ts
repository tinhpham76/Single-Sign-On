import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ListIdentityResourceComponent } from './list-identity-resource.component';

const routes: Routes=[
    {path:'', component: ListIdentityResourceComponent}
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ListIdentityResourcceRoutingModule{}