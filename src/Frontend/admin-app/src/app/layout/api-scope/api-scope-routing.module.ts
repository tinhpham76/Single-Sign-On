import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ApiScopeComponent } from './api-scope.component';


const routes: Routes = [
    { path: '', component: ApiScopeComponent }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ApiScopeRoutingModule { }