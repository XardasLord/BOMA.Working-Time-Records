import { Component, OnInit } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { RegisterCommand } from '../../models/register.command';
import { ToastrService } from 'ngx-toastr';

@Component({
	selector: 'app-register-user',
	templateUrl: './register-user.component.html',
	styleUrl: './register-user.component.scss'
})
export class RegisterUserComponent implements OnInit {
	registerForm!: FormGroup;

	constructor(
		private authService: AuthService,
		private toastService: ToastrService
	) {}

	ngOnInit(): void {
		this.registerForm = new FormGroup({
			email: new FormControl('', [Validators.required, Validators.email]),
			password: new FormControl('', [Validators.required]),
			confirm: new FormControl('')
		});
	}

	public validateControl(controlName: string) {
		return this.registerForm.get(controlName)?.invalid && this.registerForm.get(controlName)?.touched;
	}

	public hasError(controlName: string, errorName: string) {
		return this.registerForm.get(controlName)?.hasError(errorName);
	}

	public registerUser(registerFormValue: any) {
		const formValues = { ...registerFormValue };

		const user: RegisterCommand = {
			email: formValues.email,
			password: formValues.password,
			confirmPassword: formValues.confirm
		};

		this.authService.register(user).subscribe({
			next: (_) =>
				this.toastService.success('Poczekaj, aż Admin systemu zatwierdzi konto i zaloguj się.', 'Konto zarejestowano pomyślnie'),
			error: (err: HttpErrorResponse) => this.toastService.error(err.error.errors, 'Błąd podczas rejestracji konta')
		});
	}
}
