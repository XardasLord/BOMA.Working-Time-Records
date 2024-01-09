import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { NgxsModule } from '@ngxs/store';
import { NgxsFormPluginModule } from '@ngxs/form-plugin';
import { AuthRoutingModule } from './auth-routing.module';
import { AuthService } from './services/auth.service';
import { RegisterUserComponent } from './components/register-user/register-user.component';
import { LoginUserComponent } from './components/login-user/login-user.component';
import { CommonModule } from '@angular/common';
import { AppMaterialModule } from '../modules/app-material.module';
import { AuthState } from './state/auth.state';

@NgModule({
	declarations: [RegisterUserComponent, LoginUserComponent],
	imports: [
		CommonModule,
		ReactiveFormsModule,
		AppMaterialModule,
		AuthRoutingModule,
		NgxsModule.forFeature([AuthState]),
		NgxsFormPluginModule
	],
	exports: [CommonModule, ReactiveFormsModule, AppMaterialModule],
	providers: [AuthService]
})
export class AuthModule {}
