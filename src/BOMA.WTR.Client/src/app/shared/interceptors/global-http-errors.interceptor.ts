import { Injectable } from '@angular/core';
import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class GlobalHttpErrorsInterceptor implements HttpInterceptor {
	constructor(private toastService: ToastrService) {}

	intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
		return next.handle(req).pipe(
			catchError((error: HttpErrorResponse) => {
				if (error.status === 400) {
					this.toastService.error(error.error?.Message);
				}
				return throwError(() => error);
			})
		);
	}
}
