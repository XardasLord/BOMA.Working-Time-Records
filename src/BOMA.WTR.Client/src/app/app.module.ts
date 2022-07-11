import { LOCALE_ID, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CoreModule } from './core/core.module';
import { SharedModule } from './shared/shared.module';

import localePl from '@angular/common/locales/pl';
import { registerLocaleData } from '@angular/common';
import { IProgressSpinnerService } from './shared/services/progress-spinner.base.service';
import { ProgressSpinnerService } from './shared/services/progress-spinner.service';

registerLocaleData(localePl);

@NgModule({
	declarations: [AppComponent],
	imports: [BrowserModule, BrowserAnimationsModule, CoreModule, SharedModule],
	providers: [
		{ provide: LOCALE_ID, useValue: 'pl-PL' },
		{ provide: IProgressSpinnerService, useClass: ProgressSpinnerService }
	],
	bootstrap: [AppComponent]
})
export class AppModule {}
