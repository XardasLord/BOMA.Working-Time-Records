import { Component, OnInit } from '@angular/core';
import { Store } from '@ngxs/store';
import { nameof } from '../../../shared/helpers/name-of.helper';
import { UserDetails } from '../../../shared/auth/models/userDetails';
import { Users } from '../../state/users.action';
import { UsersState } from '../../state/users.state';
import Deactivate = Users.Deactivate;
import Activate = Users.Activate;
import GetAll = Users.GetAll;
import { AuthState } from '../../../shared/auth/state/auth.state';

@Component({
	selector: 'app-users-list',
	templateUrl: './users-list.component.html',
	styleUrl: './users-list.component.scss'
})
export class UsersListComponent implements OnInit {
	users$ = this.store.select(UsersState.getUsers);
	myAccount$ = this.store.select(AuthState.getUser);

	columnsToDisplay: string[] = [nameof<UserDetails>('email'), nameof<UserDetails>('role'), nameof<UserDetails>('activated'), 'actions'];

	constructor(private store: Store) {}

	ngOnInit(): void {
		this.refresh();
	}

	activate(user: UserDetails) {
		this.store.dispatch(new Activate(user.id));
	}

	deactivate(user: UserDetails) {
		this.store.dispatch(new Deactivate(user.id));
	}

	refresh() {
		this.store.dispatch(new GetAll());
	}
}
