import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { LoginCommand } from '../../models/login.command';
import { ToastrService } from 'ngx-toastr';
import { Store } from '@ngxs/store';
import { Navigate } from '@ngxs/router-plugin';
import { RoutePaths } from '../../../../core/modules/app-routing.module';
import { Auth } from '../../state/auth.action';
import Login = Auth.Login;

@Component({
	selector: 'app-login-user',
	templateUrl: './login-user.component.html',
	styleUrl: './login-user.component.scss'
})
export class LoginUserComponent implements OnInit {
	private returnUrl: string = '';

	loginForm!: FormGroup;

	constructor(
		private store: Store,
		private authService: AuthService,
		private router: Router,
		private route: ActivatedRoute,
		private toastService: ToastrService
	) {}

	ngOnInit(): void {
		this.loginForm = new FormGroup({
			username: new FormControl('', [Validators.required]),
			password: new FormControl('', [Validators.required])
		});

		this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
	}
	validateControl(controlName: string) {
		return this.loginForm.get(controlName)?.invalid && this.loginForm.get(controlName)?.touched;
	}
	hasError(controlName: string, errorName: string) {
		return this.loginForm.get(controlName)?.hasError(errorName);
	}

	loginUser(loginFormValue: any) {
		const login = { ...loginFormValue };

		const command: LoginCommand = {
			email: login.username,
			password: login.password
		};

		this.store.dispatch(new Login(command));
	}

	redirectToRegister() {
		this.store.dispatch(new Navigate([RoutePaths.Register]));
	}
}
