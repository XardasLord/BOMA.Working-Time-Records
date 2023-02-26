import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ToastrModule } from 'ngx-toastr';
import { AppMaterialModule } from './modules/app-material.module';
import { DebounceDirective } from './directives/debounce.directive';
import { YearFormatDirective } from './directives/date-picket-year-format.directive';
import { MonthFormatDirective } from './directives/date-picker-month-format.directive';
import { ProgressSpinnerDialogComponent } from './ui/components/progress-spinner/progress-spinner-dialog.component';
import { EmptyIfZeroPipe } from './pipes/empty-if-zero.pipe';
import { NgxPrintModule } from 'ngx-print';
import { ConfirmationDialogComponent } from './components/confirmation-dialog/confirmation-dialog.component';

@NgModule({
	declarations: [DebounceDirective, YearFormatDirective, MonthFormatDirective, EmptyIfZeroPipe, ProgressSpinnerDialogComponent, ConfirmationDialogComponent],
	imports: [CommonModule, FormsModule, ReactiveFormsModule, HttpClientModule, AppMaterialModule, ToastrModule.forRoot()],
	exports: [
		CommonModule,
		FormsModule,
		ReactiveFormsModule,
		HttpClientModule,
		AppMaterialModule,
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
