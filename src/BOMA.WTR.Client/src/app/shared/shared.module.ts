import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppMaterialModule } from './modules/app-material.module';
import { ToastrModule } from 'ngx-toastr';
import { HttpClientModule } from '@angular/common/http';
import { DebounceDirective } from './directives/debounce.directive';
import { YearFormatDirective } from './directives/date-picket-year-format.directive';
import { MonthFormatDirective } from './directives/date-picker-month-format.directive';

@NgModule({
	declarations: [DebounceDirective, YearFormatDirective, MonthFormatDirective],
	imports: [CommonModule, FormsModule, ReactiveFormsModule, HttpClientModule, AppMaterialModule, ToastrModule.forRoot()],
	exports: [
		CommonModule,
		FormsModule,
		ReactiveFormsModule,
		HttpClientModule,
		AppMaterialModule,
		ToastrModule,
		DebounceDirective,
		YearFormatDirective,
		MonthFormatDirective
	],
	providers: []
})
export class SharedModule {}
