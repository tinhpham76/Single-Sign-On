import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ApiResourceComponent } from './api-resource.component';
import { AddResourceComponent } from './add-resource/add-resource.component';

const routes: Routes = [
    { path: '', component: ApiResourceComponent },
    { path: 'add', component: AddResourceComponent },
    { path: ':name/settings', loadChildren: () => import('./setting-resource/setting-resource.module').then(m => m.SettingResourceModule) }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ApiResourceRoutingModule { }