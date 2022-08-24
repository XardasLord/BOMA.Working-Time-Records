import { MAT_DATE_FORMATS } from '@angular/material/core';
import { Directive } from '@angular/core';

export const FORMAT = {
	parse: {
		dateInput: 'MM'
	},
	display: {
		dateInput: 'MM',
		monthYearLabel: 'MMM',
		dateA11yLabel: 'LL',
		monthYearA11yLabel: 'MMMM'
	}
};
@Directive({
	selector: '[appMonthFormat]',
	providers: [{ provide: MAT_DATE_FORMATS, useValue: FORMAT }]
})
export class MonthFormatDirective {}
