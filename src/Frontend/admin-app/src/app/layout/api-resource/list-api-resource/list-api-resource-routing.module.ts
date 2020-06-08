import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ListApiResourceComponent } from './list-api-resource.component';

const routes: Routes = [
    {path: '', component: ListApiResourceComponent}
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ListApiResourceRoutingModule{}