import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AuthRoutingModule } from './auth-routing.module';
import { SignInComponent } from './sign-in/sign-in.component';
import { SignUpComponent } from './sign-up/sign-up.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { RequestPasswordComponent } from './request-password/request-password.component';


@NgModule({
  declarations: [
    SignInComponent,
    SignUpComponent,
    ResetPasswordComponent,
    RequestPasswordComponent
  ],
  imports: [
    CommonModule,
    AuthRoutingModule
  ]
})
export class AuthModule { }
