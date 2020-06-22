import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserComponent } from './user.component';
import { AddUserComponent } from './add-user/add-user.component';
import { UserRoleComponent } from './user-role/user-role.component';

const routes: Routes = [
    { path: '', component: UserComponent },
    { path: 'add', component: AddUserComponent },
    { path: ':userId/user-role', component: UserRoleComponent}
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class UserRoutingModule { }