import { Component } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { environment } from '../../../../../environments/environment';
import { Auth } from '../../../../shared/auth/state/auth.action';
import Logout = Auth.Logout;
import { AuthState } from '../../../../shared/auth/state/auth.state';

@Component({
	selector: 'app-navigation',
	templateUrl: './navigation.component.html',
	styleUrls: ['./navigation.component.scss']
})
export class NavigationComponent {
	appVersion: string = environment.appVersion;
	isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset).pipe(
		map((result) => result.matches),
		shareReplay()
	);

	loggedIn$ = this.store.select(AuthState.isLoggedIn);
	user$ = this.store.select(AuthState.getUser);

	constructor(
		private breakpointObserver: BreakpointObserver,
		private store: Store
	) {}

	logout() {
		this.store.dispatch(new Logout());
	}
}
