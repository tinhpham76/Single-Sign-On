import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EditIdentityResourceComponent } from './edit-identity-resource.component';

const routes: Routes=[
    {path: '', component: EditIdentityResourceComponent}
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class EditIdentityResourceRoutingModule{}