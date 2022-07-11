import { Injectable } from '@angular/core';
import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { IProgressSpinnerService } from '../services/progress-spinner.base.service';

@Injectable()
export class GlobalHttpErrorsInterceptor implements HttpInterceptor {
	constructor(private toastService: ToastrService, private progressSpinnerService: IProgressSpinnerService) {}

	intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
		return next.handle(request).pipe(
			catchError((error: HttpErrorResponse) => {
				console.log('Passed through the interceptor in request');

				if (error.status === 400) {
					this.toastService.error(error.error?.Message);
				}

				this.progressSpinnerService.hideProgressSpinner();
				return throwError(() => error);
			})
		);
	}
}
