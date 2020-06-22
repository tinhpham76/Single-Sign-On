import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IdentityResourceComponent } from './identity-resource.component';
import { AddIdentityResourceComponent } from './add-identity-resource/add-identity-resource.component';
import { IdentityClaimComponent } from './identity-claim/identity-claim.component';

const routes: Routes = [
    { path: '', component: IdentityResourceComponent },
    { path: 'add', component: AddIdentityResourceComponent },
    { path: ':name/claim', component: IdentityClaimComponent }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class IdentityResourceRoutingModule { }