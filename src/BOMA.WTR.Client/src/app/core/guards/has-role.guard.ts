import { CanActivateFn } from '@angular/router';
import { Store } from '@ngxs/store';
import { inject } from '@angular/core';
import { Navigate } from '@ngxs/router-plugin';
import { AuthState } from '../../shared/auth/state/auth.state';
import { RoutePaths } from '../modules/app-routing.module';
import { Auth } from '../../shared/auth/state/auth.action';
import Logout = Auth.Logout;

export const HasRoleGuard: CanActivateFn = (route, state) => {
	const store: Store = inject(Store);
	const isLoggedIn = store.selectSnapshot(AuthState.isLoggedIn);
	const userRole = store.selectSnapshot(AuthState.getUser)?.role;
	const expectedRoles: string[] = route.data['roles'];

	if (!isLoggedIn) {
		return store.dispatch(new Logout());
	}

	const hasRole: boolean = expectedRoles.some((role) => userRole === role);

	return hasRole || store.dispatch(new Navigate([RoutePaths.WorkingTimeRecords]));
};
