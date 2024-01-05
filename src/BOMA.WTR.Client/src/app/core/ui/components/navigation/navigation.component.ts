import { Component } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { environment } from '../../../../../environments/environment';
import { AuthService } from '../../../../shared/auth/services/auth.service';
import { Store } from '@ngxs/store';
import { Navigate } from '@ngxs/router-plugin';
import { RoutePaths } from '../../../modules/app-routing.module';

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

	constructor(
		private breakpointObserver: BreakpointObserver,
		private authService: AuthService,
		private store: Store
	) {}

	isLoggedIn(): boolean {
		return !!localStorage.getItem('boma_ecp_token');
	}

	logout() {
		this.authService.logout().subscribe(() => {
			localStorage.removeItem('boma_ecp_token');

			this.store.dispatch(new Navigate([RoutePaths.Login]));
		});
	}
}
