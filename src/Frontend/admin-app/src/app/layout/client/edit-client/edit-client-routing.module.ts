import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EditClientComponent } from './edit-client.component';

const routes: Routes=[
    {path: '', component: EditClientComponent}
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class EditClientRoutingModule{}