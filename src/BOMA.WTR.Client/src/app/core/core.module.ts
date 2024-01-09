import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LayoutModule } from '@angular/cdk/layout';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppRoutingModule } from './modules/app-routing.module';
import { AppNgxsModule } from './modules/app-ngxs.module';
import { NavigationComponent } from './ui/components/navigation/navigation.component';
import { GlobalHttpErrorsInterceptor } from '../shared/interceptors/global-http-errors.interceptor';
import { SharedModule } from '../shared/shared.module';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { UnauthorizedInterceptor } from './interceptors/unauthorized.interceptor';

@NgModule({
	declarations: [NavigationComponent],
	imports: [CommonModule, LayoutModule, AppRoutingModule, AppNgxsModule, SharedModule],
	exports: [CommonModule, LayoutModule, AppRoutingModule, NavigationComponent],
	providers: [
		{
			provide: HTTP_INTERCEPTORS,
			useClass: GlobalHttpErrorsInterceptor,
			multi: true
		},
		{
			provide: HTTP_INTERCEPTORS,
			useClass: AuthInterceptor,
			multi: true
		},
		{
			provide: HTTP_INTERCEPTORS,
			useClass: UnauthorizedInterceptor,
			multi: true
		}
	]
})
export class CoreModule {}
