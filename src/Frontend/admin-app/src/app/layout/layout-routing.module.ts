import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LayoutComponent } from './layout.component';

const routes: Routes = [
    {
        path: '',
        component: LayoutComponent,
        children: [
            { path: '', redirectTo: 'home', pathMatch: 'prefix' },
            { path: 'home', loadChildren: () => import('./home/home.module').then(m => m.HomeModule) },
            { path: 'clients', loadChildren: () => import('./client/client.module').then(m => m.ClientModule) },
            { path: 'api-resources', loadChildren: () => import('./api-resource/api-resource.module').then(m => m.ApiResourceModule) },
            { path: 'api-scopes', loadChildren: () => import('./api-scope/api-scope.module').then(m => m.ApiScopeModule) },
            {
                path: 'identity-resources', loadChildren: () =>
                    import('./identity-resource/identity-resource.module').then(m => m.IdentityResourceModule)
            },
            { path: 'users', loadChildren: () => import('./user/user.module').then(m => m.UserModule) },
            { path: 'roles', loadChildren: () => import('./role/role.module').then(m => m.RoleModule) }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class LayoutRoutingModule { }
