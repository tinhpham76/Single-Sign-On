import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddApiResourceComponent } from './add-api-resource.component';

const routes: Routes=[
    {path: '', component: AddApiResourceComponent}
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AddApiResourceRoutingModule{}