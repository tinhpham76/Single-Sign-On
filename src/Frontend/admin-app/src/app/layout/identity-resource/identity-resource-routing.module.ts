import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IdentityResourceComponent } from './identity-resource.component';

const routes: Routes=[
    {path: '', component: IdentityResourceComponent}
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class IdentityResourceRoutingModule{}