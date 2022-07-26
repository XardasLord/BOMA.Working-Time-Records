import { FormControl } from '@angular/forms';

export interface WorkingTimeRecordSummaryDataFormModel {
	employeeId: number;
	holidaySalary: number;
	sicknessSalary: number;
	additionalSalary: number;
	year: number;
	month: number;
}

export interface WorkingTimeRecordDetailedDataFormModel {
	year: number;
	month: number;
	employeeId: number;
	day1: number;
	day2: number;
	day3: number;
	day4: number;
	day5: number;
	day6: number;
	day7: number;
	day8: number;
	day9: number;
	day10: number;
	day11: number;
	day12: number;
	day13: number;
	day14: number;
	day15: number;
	day16: number;
	day17: number;
	day18: number;
	day19: number;
	day20: number;
	day21: number;
	day22: number;
	day23: number;
	day24: number;
	day25: number;
	day26: number;
	day27: number;
	day28: number;
	day29: number;
	day30: number;
	day31: number;
}

export interface WorkingTimeRecordDetailedFormGroup {
	year: FormControl<number>;
	month: FormControl<number>;
	employeeId: FormControl<number>;
	day1: FormControl<number>;
	day2: FormControl<number>;
	day3: FormControl<number>;
	day4: FormControl<number>;
	day5: FormControl<number>;
	day6: FormControl<number>;
	day7: FormControl<number>;
	day8: FormControl<number>;
	day9: FormControl<number>;
	day10: FormControl<number>;
	day11: FormControl<number>;
	day12: FormControl<number>;
	day13: FormControl<number>;
	day14: FormControl<number>;
	day15: FormControl<number>;
	day16: FormControl<number>;
	day17: FormControl<number>;
	day18: FormControl<number>;
	day19: FormControl<number>;
	day20: FormControl<number>;
	day21: FormControl<number>;
	day22: FormControl<number>;
	day23: FormControl<number>;
	day24: FormControl<number>;
	day25: FormControl<number>;
	day26: FormControl<number>;
	day27: FormControl<number>;
	day28: FormControl<number>;
	day29: FormControl<number>;
	day30: FormControl<number>;
	day31: FormControl<number>;
}
