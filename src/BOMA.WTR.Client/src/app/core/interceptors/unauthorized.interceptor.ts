import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse } from '@angular/common/http';
import { Store } from '@ngxs/store';
import { Navigate } from '@ngxs/router-plugin';
import { Observable, tap } from 'rxjs';
import { RoutePaths } from '../modules/app-routing.module';

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
						// this.store.dispatch(new Relog());
						this.store.dispatch(new Navigate([RoutePaths.Login]));
					}
				}
			)
		);
	}
}
