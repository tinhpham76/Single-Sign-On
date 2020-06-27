import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SettingScopeComponent } from './setting-scope.component';
import { PropertyComponent } from './property/property.component';

const routes: Routes = [
    { path: '', component: SettingScopeComponent },
    { path: 'properties', component: PropertyComponent}
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class SettingScopeRoutingModule { }