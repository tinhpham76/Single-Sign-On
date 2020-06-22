import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ApiResourceComponent } from './api-resource.component';
import { AddApiComponent } from './add-api/add-api.component';
import { SettingComponent } from './setting/setting.component';

const routes: Routes = [
    { path: '', component: ApiResourceComponent },
    { path: 'add', component: AddApiComponent },
    { path: ':name/settings', loadChildren: () => import('./setting/setting.module').then(m => m.SettingModule) }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ApiResourceRoutingModule { }