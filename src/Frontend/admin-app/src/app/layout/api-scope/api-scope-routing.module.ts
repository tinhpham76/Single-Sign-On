import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ApiScopeComponent } from './api-scope.component';
import { AddScopeComponent } from './add-scope/add-scope.component';



const routes: Routes = [
    { path: '', component: ApiScopeComponent },
    { path: 'add', component: AddScopeComponent},
    { path: ':name/settings', loadChildren: () => import('./setting-scope/setting-scope.module').then(m => m.SettingScopeModule) }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ApiScopeRoutingModule { }