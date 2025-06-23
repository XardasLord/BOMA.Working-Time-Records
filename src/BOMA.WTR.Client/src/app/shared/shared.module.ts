import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { provideStates } from '@ngxs/store';
import { ToastrModule } from 'ngx-toastr';
import { NgxPrintModule } from 'ngx-print';
import { DebounceDirective } from './directives/debounce.directive';
import { YearFormatDirective } from './directives/date-picket-year-format.directive';
import { MonthFormatDirective } from './directives/date-picker-month-format.directive';
import { ProgressSpinnerDialogComponent } from './ui/components/progress-spinner/progress-spinner-dialog.component';
import { EmptyIfZeroPipe } from './pipes/empty-if-zero.pipe';
import { ConfirmationDialogComponent } from './components/confirmation-dialog/confirmation-dialog.component';
import { AuthModule } from './auth/auth.module';
import { SettingsState } from '../settings/state/settings.state';
import { SettingsService } from '../settings/services/settings.service';

@NgModule({
	declarations: [
		DebounceDirective,
		YearFormatDirective,
		MonthFormatDirective,
		EmptyIfZeroPipe,
		ProgressSpinnerDialogComponent,
		ConfirmationDialogComponent
	],
	exports: [
		FormsModule,
		AuthModule,
		ToastrModule,
		NgxPrintModule,
		DebounceDirective,
		YearFormatDirective,
		MonthFormatDirective,
		EmptyIfZeroPipe
	],
	imports: [FormsModule, AuthModule, ToastrModule.forRoot()],
	providers: [provideHttpClient(withInterceptorsFromDi()), provideStates([SettingsState]), SettingsService]
})
export class SharedModule {}
