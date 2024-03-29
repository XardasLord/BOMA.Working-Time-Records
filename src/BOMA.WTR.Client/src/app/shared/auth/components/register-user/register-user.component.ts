import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngxs/store';
import { Navigate } from '@ngxs/router-plugin';
import { RegisterCommand } from '../../models/register.command';
import { Auth } from '../../state/auth.action';
import { RoutePaths } from '../../../../core/modules/app-routing.module';
import Register = Auth.Register;

@Component({
	selector: 'app-register-user',
	templateUrl: './register-user.component.html',
	styleUrl: './register-user.component.scss'
})
export class RegisterUserComponent implements OnInit {
	registerForm!: FormGroup;

	constructor(private store: Store) {}

	ngOnInit(): void {
		this.registerForm = new FormGroup({
			email: new FormControl('', [Validators.required, Validators.email]),
			password: new FormControl('', [Validators.required]),
			confirm: new FormControl('')
		});
	}

	public registerUser(registerFormValue: any) {
		const formValues = { ...registerFormValue };

		const command: RegisterCommand = {
			email: formValues.email,
			password: formValues.password,
			confirmPassword: formValues.confirm
		};

		this.store.dispatch(new Register(command));
	}

	public redirectToLogin() {
		this.store.dispatch(new Navigate([RoutePaths.Login]));
	}
}
