import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ToastrModule } from 'ngx-toastr';
import { DebounceDirective } from './directives/debounce.directive';
import { YearFormatDirective } from './directives/date-picket-year-format.directive';
import { MonthFormatDirective } from './directives/date-picker-month-format.directive';
import { ProgressSpinnerDialogComponent } from './ui/components/progress-spinner/progress-spinner-dialog.component';
import { EmptyIfZeroPipe } from './pipes/empty-if-zero.pipe';
import { NgxPrintModule } from 'ngx-print';
import { ConfirmationDialogComponent } from './components/confirmation-dialog/confirmation-dialog.component';
import { AuthModule } from './auth/auth.module';

@NgModule({
	declarations: [
		DebounceDirective,
		YearFormatDirective,
		MonthFormatDirective,
		EmptyIfZeroPipe,
		ProgressSpinnerDialogComponent,
		ConfirmationDialogComponent
	],
	imports: [FormsModule, HttpClientModule, AuthModule, ToastrModule.forRoot()],
	exports: [
		FormsModule,
		HttpClientModule,
		AuthModule,
		ToastrModule,
		NgxPrintModule,
		DebounceDirective,
		YearFormatDirective,
		MonthFormatDirective,
		EmptyIfZeroPipe
	],
	providers: []
})
export class SharedModule {}
