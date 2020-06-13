import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ApiResourceComponent } from './api-resource.component';

const routes: Routes = [
    { path: '', component: ApiResourceComponent }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ApiResourceRoutingModule { }