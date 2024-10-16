import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { Navigate } from '@ngxs/router-plugin';
import { Observable, of } from 'rxjs';
import { AuthState } from '../../shared/auth/state/auth.state';
import { RoutePaths } from '../modules/app-routing.module';
import { Auth } from '../../shared/auth/state/auth.action';
import Logout = Auth.Logout;

@Injectable({
	providedIn: 'root'
})
export class HasRoleGuard implements CanActivate {
	constructor(private store: Store) {}

	isLoggedIn = this.store.selectSnapshot(AuthState.isLoggedIn);
	userRole = this.store.selectSnapshot(AuthState.getUser)?.role;

	canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
		if (!this.isLoggedIn) {
			this.store.dispatch(new Logout());

			return of(false);
		}

		const expectedRoles: string[] = route.data!['roles'];

		const hasRole: boolean = expectedRoles.some((role) => this.userRole === role);

		if (!hasRole) {
			this.store.dispatch(new Navigate([RoutePaths.WorkingTimeRecords]));
			return of(false);
		}

		return of(true);
	}
}
