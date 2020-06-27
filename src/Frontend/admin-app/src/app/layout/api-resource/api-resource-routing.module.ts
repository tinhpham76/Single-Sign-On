import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ApiResourceComponent } from './api-resource.component';
import { AddResourceComponent } from './add-resource/add-resource.component';

const routes: Routes = [
    { path: '', component: ApiResourceComponent },
    { path: 'add', component: AddResourceComponent },
    { path: ':name/settings', loadChildren: () => import('./setting/setting.module').then(m => m.SettingModule) }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ApiResourceRoutingModule { }