import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { LayoutModule } from '@angular/cdk/layout';
import { AppRoutingModule } from './modules/app-routing.module';
import { AppNgxsModule } from './modules/app-ngxs.module';
import { AppMaterialModule } from './modules/app-material.module';
import { NavigationComponent } from './ui/components/navigation/navigation.component';
import { ToastrModule } from 'ngx-toastr';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { GlobalHttpErrorsInterceptor } from '../shared/interceptors/global-http-errors.interceptor';

@NgModule({
	declarations: [NavigationComponent],
	imports: [
		CommonModule,
		LayoutModule,
		HttpClientModule,
		AppMaterialModule,
		AppRoutingModule,
		AppNgxsModule,
		ReactiveFormsModule,
		ToastrModule.forRoot()
	],
	exports: [CommonModule, HttpClientModule, AppRoutingModule, AppMaterialModule, NavigationComponent, ReactiveFormsModule, ToastrModule],
	providers: [
		{
			provide: HTTP_INTERCEPTORS,
			useClass: GlobalHttpErrorsInterceptor,
			multi: true
		}
	]
})
export class CoreModule {}
