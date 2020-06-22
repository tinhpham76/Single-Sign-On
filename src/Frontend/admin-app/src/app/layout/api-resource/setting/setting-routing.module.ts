import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SettingComponent } from './setting.component';
import { ScopeComponent } from './scope/scope.component';
import { SecretComponent } from './secret/secret.component';

const routes: Routes = [
    { path: '', component: SettingComponent },
    { path: 'scopes', component: ScopeComponent},
    { path: 'secrets', component: SecretComponent}

];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class SettingRoutingModule { }