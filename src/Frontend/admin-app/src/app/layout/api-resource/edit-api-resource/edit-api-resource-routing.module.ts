import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EditApiResourceComponent } from './edit-api-resource.component';

const routes: Routes=[
    {path: '', component: EditApiResourceComponent}
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class EditApiResourceRoutingModule{}