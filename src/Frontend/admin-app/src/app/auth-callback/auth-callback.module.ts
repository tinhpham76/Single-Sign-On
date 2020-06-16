import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthCallbackComponent } from './auth-callback.component';
import { AuthCallbackRoutingModule } from './auth-callback-routing.module';


@NgModule({
  declarations: [AuthCallbackComponent],
  imports: [
    CommonModule,
    AuthCallbackRoutingModule
  ],
  bootstrap: [AuthCallbackComponent]
})
export class AuthCallbackModule { }
