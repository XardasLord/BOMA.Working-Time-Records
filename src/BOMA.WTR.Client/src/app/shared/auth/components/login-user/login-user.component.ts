import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngxs/store';
import { Navigate } from '@ngxs/router-plugin';
import { ActivatedRoute } from '@angular/router';
import { LoginCommand } from '../../models/login.command';
import { RoutePaths } from '../../../../core/modules/app-routing.module';
import { Auth } from '../../state/auth.action';
import Login = Auth.Login;

@Component({
	selector: 'app-login-user',
	templateUrl: './login-user.component.html',
	styleUrl: './login-user.component.scss'
})
export class LoginUserComponent implements OnInit {
	loginForm!: FormGroup;

	constructor(
		private store: Store,
		private route: ActivatedRoute
	) {}

	ngOnInit(): void {
		this.loginForm = new FormGroup({
			email: new FormControl('', [Validators.required]),
			password: new FormControl('', [Validators.required])
		});
	}

	loginUser(loginFormValue: any) {
		const login = { ...loginFormValue };

		const command: LoginCommand = {
			email: login.email,
			password: login.password
		};

		this.store.dispatch(new Login(command));
	}

	redirectToRegister() {
		this.store.dispatch(new Navigate([RoutePaths.Register]));
	}
}
