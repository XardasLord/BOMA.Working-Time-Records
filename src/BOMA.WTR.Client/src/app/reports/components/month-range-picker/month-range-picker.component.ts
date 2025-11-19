import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepicker, MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { MatIconModule } from '@angular/material/icon';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MomentDateAdapter, MAT_MOMENT_DATE_ADAPTER_OPTIONS, MatMomentDateModule } from '@angular/material-moment-adapter';
import moment from 'moment';
import 'moment/locale/pl';

export interface MonthRange {
	start: Date;
	end: Date;
}

export const MY_FORMATS = {
	parse: {
		dateInput: 'MM.YYYY'
	},
	display: {
		dateInput: 'MM.YYYY',
		monthYearLabel: 'MMM YYYY',
		dateA11yLabel: 'LL',
		monthYearA11yLabel: 'MMMM YYYY'
	}
};

@Component({
	selector: 'app-month-range-picker',
	standalone: true,
	imports: [
		CommonModule,
		MatFormFieldModule,
		MatDatepickerModule,
		MatInputModule,
		MatMomentDateModule,
		MatIconModule,
		ReactiveFormsModule
	],
	providers: [
		{
			provide: DateAdapter,
			useClass: MomentDateAdapter,
			deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS]
		},
		{ provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },
		{ provide: MAT_DATE_LOCALE, useValue: 'pl' }
	],
	templateUrl: './month-range-picker.component.html',
	styleUrls: ['./month-range-picker.component.scss']
})
export class MonthRangePickerComponent implements OnInit {
	@Input() startDate: Date | null = null;
	@Input() endDate: Date | null = null;
	@Output() dateRangeChanged = new EventEmitter<MonthRange>();

	startMonthControl = new FormControl<moment.Moment | null>(null);
	endMonthControl = new FormControl<moment.Moment | null>(null);

	constructor() {
		moment.locale('pl');
	}

	ngOnInit(): void {
		if (this.startDate && this.endDate) {
			this.startMonthControl.setValue(moment(this.startDate));
			this.endMonthControl.setValue(moment(this.endDate));
		}
	}

	onStartMonthSelected(normalizedMonth: moment.Moment, datepicker: MatDatepicker<moment.Moment>): void {
		const selectedMonth = moment(normalizedMonth).startOf('month');
		this.startMonthControl.setValue(selectedMonth);

		datepicker.close();

		// Jeśli mamy już koniec i początek jest po końcu, zamień
		if (this.endMonthControl.value) {
			if (selectedMonth.isAfter(this.endMonthControl.value, 'month')) {
				this.endMonthControl.setValue(selectedMonth.clone().endOf('month'));
			}
			this.emitDateRange();
		}
	}

	onEndMonthSelected(normalizedMonth: moment.Moment, datepicker: MatDatepicker<moment.Moment>): void {
		const selectedMonth = moment(normalizedMonth).endOf('month');
		this.endMonthControl.setValue(selectedMonth);

		datepicker.close();

		// Jeśli mamy już początek i koniec jest przed początkiem, zamień
		if (this.startMonthControl.value) {
			if (selectedMonth.isBefore(this.startMonthControl.value, 'month')) {
				this.startMonthControl.setValue(selectedMonth.clone().startOf('month'));
			}
			this.emitDateRange();
		}
	}

	private emitDateRange(): void {
		const start = this.startMonthControl.value;
		const end = this.endMonthControl.value;

		if (start && end) {
			this.dateRangeChanged.emit({
				start: start.toDate(),
				end: end.toDate()
			});
		}
	}
}
