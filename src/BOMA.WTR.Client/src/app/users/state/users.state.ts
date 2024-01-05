import { Injectable } from '@angular/core';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { catchError, finalize, Observable, tap, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { UserDetails } from '../../shared/auth/models/userDetails';
import { UsersService } from '../services/users.service';
import { IProgressSpinnerService } from '../../shared/services/progress-spinner.base.service';
import { Users } from './users.action';
import GetUsers = Users.GetAll;

export interface UsersStateModel {
	users: UserDetails[];
}

const USERS_STATE_TOKEN = new StateToken<UsersStateModel>('users');

@State<UsersStateModel>({
	name: USERS_STATE_TOKEN,
	defaults: {
		users: []
	}
})
@Injectable()
export class UsersState {
	constructor(
		private usersService: UsersService,
		private toastService: ToastrService,
		private progressSpinnerService: IProgressSpinnerService
	) {}

	@Selector([USERS_STATE_TOKEN])
	static getUsers(state: UsersStateModel): UserDetails[] {
		return state.users;
	}

	@Action(GetUsers)
	getUsers(ctx: StateContext<UsersStateModel>, _: GetUsers): Observable<UserDetails[]> {
		this.progressSpinnerService.showProgressSpinner();

		return this.usersService.getUsers().pipe(
			tap((response) => {
				ctx.patchState({
					users: response
				});
			}),
			catchError((e) => {
				return throwError(e);
			}),
			finalize(() => {
				this.progressSpinnerService.hideProgressSpinner();
			})
		);
	}
}
