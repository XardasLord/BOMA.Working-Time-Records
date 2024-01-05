import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse } from '@angular/common/http';
import { Store } from '@ngxs/store';
import { Observable, tap } from 'rxjs';
import { Auth } from '../../shared/auth/state/auth.action';
import Logout = Auth.Logout;

@Injectable()
export class UnauthorizedInterceptor implements HttpInterceptor {
	constructor(private store: Store) {}

	intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
		return next.handle(request).pipe(
			tap(
				(event: HttpEvent<any>) => {
					if (event instanceof HttpResponse) {
						// Tutaj możesz przetwarzać odpowiedź HTTP
					}
				},
				(error) => {
					if (error.status === 401) {
						this.store.dispatch(new Logout());
					}
				}
			)
		);
	}
}
