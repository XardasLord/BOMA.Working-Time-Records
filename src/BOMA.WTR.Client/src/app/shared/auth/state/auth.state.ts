import { Injectable } from '@angular/core';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { Navigate } from '@ngxs/router-plugin';
import { catchError, finalize, Observable, tap, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../services/auth.service';
import { IProgressSpinnerService } from '../../services/progress-spinner.base.service';
import { Auth } from './auth.action';
import { UserDetails } from '../models/userDetails';
import Register = Auth.Register;
import { RegisterResponse } from '../models/register.response';
import Login = Auth.Login;
import { LoginResponse } from '../models/login.response';
import Logout = Auth.Logout;
import { RoutePaths } from '../../../core/modules/app-routing.module';
import GetMyRole = Auth.GetMyAccountDetails;
import GetMyAccountDetails = Auth.GetMyAccountDetails;

export interface AuthStateModel {
	loggedIn: boolean;
	accessToken: string | null;
	user: UserDetails | null;
}

const AUTH_STATE_TOKEN = new StateToken<AuthStateModel>('auth');
const accessTokenStorageKey = 'boma_ecp_token';
const userInfoStorageKey = 'boma_ecp_user';

@State<AuthStateModel>({
	name: AUTH_STATE_TOKEN,
	defaults: {
		loggedIn: !!localStorage.getItem(accessTokenStorageKey),
		accessToken: localStorage.getItem(accessTokenStorageKey),
		user: JSON.parse(localStorage.getItem(userInfoStorageKey)!)
	}
})
@Injectable()
export class AuthState {
	constructor(
		private authService: AuthService,
		private toastService: ToastrService,
		private progressSpinnerService: IProgressSpinnerService
	) {}

	@Selector([AUTH_STATE_TOKEN])
	static isLoggedIn(state: AuthStateModel): boolean {
		return state.loggedIn;
	}

	@Selector([AUTH_STATE_TOKEN])
	static getToken(state: AuthStateModel): string | null {
		return state.accessToken;
	}

	@Selector([AUTH_STATE_TOKEN])
	static getUser(state: AuthStateModel): UserDetails | null {
		return state.user;
	}

	@Action(Register)
	register(ctx: StateContext<AuthStateModel>, action: Register): Observable<RegisterResponse> {
		this.progressSpinnerService.showProgressSpinner();

		return this.authService.register(action.command).pipe(
			tap((response) => {
				this.toastService.success('Poczekaj, aż Admin systemu zatwierdzi konto i zaloguj się.', 'Konto zarejestowano pomyślnie'),
					ctx.dispatch(new Navigate([RoutePaths.Login]));
			}),
			catchError((e) => {
				this.toastService.error(e.error.errors, 'Błąd podczas rejestracji konta');

				return throwError(e);
			}),
			finalize(() => {
				this.progressSpinnerService.hideProgressSpinner();
			})
		);
	}

	@Action(Login)
	login(ctx: StateContext<AuthStateModel>, action: Login): Observable<LoginResponse> {
		this.progressSpinnerService.showProgressSpinner();

		return this.authService.login(action.command).pipe(
			tap((response) => {
				localStorage.setItem(accessTokenStorageKey, response.accessToken);

				ctx.patchState({
					loggedIn: true,
					accessToken: response.accessToken
				});

				ctx.dispatch(new GetMyRole());
				ctx.dispatch(new Navigate([RoutePaths.WorkingTimeRecords]));
			}),
			catchError((e) => {
				this.toastService.error('Nieprawidłowe dane do logowania lub konto nieaktywne', 'Błąd podczas logowania');

				return throwError(e);
			}),
			finalize(() => {
				this.progressSpinnerService.hideProgressSpinner();
			})
		);
	}

	@Action(Logout)
	logout(ctx: StateContext<AuthStateModel>, _: Logout) {
		this.progressSpinnerService.showProgressSpinner();

		return this.authService.logout().pipe(
			tap((response) => {
				localStorage.removeItem(accessTokenStorageKey);
				localStorage.removeItem(userInfoStorageKey);

				ctx.patchState({
					loggedIn: false,
					accessToken: null,
					user: null
				});

				ctx.dispatch(new Navigate([RoutePaths.Login]));
			}),
			catchError((e) => {
				return throwError(e);
			}),
			finalize(() => {
				this.progressSpinnerService.hideProgressSpinner();
			})
		);
	}

	@Action(GetMyAccountDetails)
	getMyAccountDetails(ctx: StateContext<AuthStateModel>, _: GetMyAccountDetails): Observable<UserDetails> {
		return this.authService.getMyAccountDetails().pipe(
			tap((response) => {
				localStorage.setItem(userInfoStorageKey, JSON.stringify(response));

				ctx.patchState({
					user: response
				});
			}),
			catchError((e) => {
				return throwError(e);
			})
		);
	}
}
