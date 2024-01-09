import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Store } from '@ngxs/store';
import { AuthState } from '../../shared/auth/state/auth.state';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
	constructor(private store: Store) {}

	intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
		const token = this.store.selectSnapshot(AuthState.getToken);

		if (token) {
			req = req.clone({
				headers: req.headers.set('Authorization', `Bearer ${token}`)
			});
		}

		return next.handle(req);
	}
}
