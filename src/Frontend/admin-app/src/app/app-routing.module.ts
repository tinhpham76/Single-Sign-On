import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './shared/guard/auth.guard';
import { AccessDeniedComponent } from './access-denied/access-denied.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { ServerErrorComponent } from './server-error/server-error.component';
const routes: Routes = [
    { path: '', loadChildren: () => import('./layout/layout.module').then(m => m.LayoutModule), canActivate: [AuthGuard] },
    { path: 'login', loadChildren: () => import('./login/login.module').then(m => m.LoginModule) },
    { path: 'auth-callback', loadChildren: () => import('./auth-callback/auth-callback.module').then(m => m.AuthCallbackModule) },
    { path: 'error', component: ServerErrorComponent },
    { path: 'access-denied', component: AccessDeniedComponent},
    { path: 'not-found', component: NotFoundComponent },
    { path: '**', redirectTo: 'not-found' }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule {}
