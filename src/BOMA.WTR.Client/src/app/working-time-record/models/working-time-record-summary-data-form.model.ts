import { FormControl } from '@angular/forms';

export interface WorkingTimeRecordSummaryDataFormModel {
	employeeId: number;
	baseSalary: number;
	percentageBonusSalary: number;
	holidaySalary: number;
	sicknessSalary: number;
	additionalSalary: number;
	year: number;
	month: number;
}

export class WorkingTimeRecordDetailedDataFormModel {
	year!: number;
	month!: number;
	employeeId!: number;
	dayHours!: Record<number, number>;
	saturdayHours!: Record<number, number>;
	nightHours!: Record<number, number>;

	constructor(init?: Partial<WorkingTimeRecordDetailedDataFormModel>) {
		Object.assign(this, { ...init });
	}
}

export class WorkingTimeRecordReportHoursDataFormModel {
	year!: number;
	month!: number;
	employeeId!: number;
	reportEntryHours!: Record<number, number>;
	reportExitHours!: Record<number, number>;

	constructor(init?: Partial<WorkingTimeRecordReportHoursDataFormModel>) {
		Object.assign(this, { ...init });
	}
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

	day1saturday: FormControl<number>;
	day2saturday: FormControl<number>;
	day3saturday: FormControl<number>;
	day4saturday: FormControl<number>;
	day5saturday: FormControl<number>;
	day6saturday: FormControl<number>;
	day7saturday: FormControl<number>;
	day8saturday: FormControl<number>;
	day9saturday: FormControl<number>;
	day10saturday: FormControl<number>;
	day11saturday: FormControl<number>;
	day12saturday: FormControl<number>;
	day13saturday: FormControl<number>;
	day14saturday: FormControl<number>;
	day15saturday: FormControl<number>;
	day16saturday: FormControl<number>;
	day17saturday: FormControl<number>;
	day18saturday: FormControl<number>;
	day19saturday: FormControl<number>;
	day20saturday: FormControl<number>;
	day21saturday: FormControl<number>;
	day22saturday: FormControl<number>;
	day23saturday: FormControl<number>;
	day24saturday: FormControl<number>;
	day25saturday: FormControl<number>;
	day26saturday: FormControl<number>;
	day27saturday: FormControl<number>;
	day28saturday: FormControl<number>;
	day29saturday: FormControl<number>;
	day30saturday: FormControl<number>;
	day31saturday: FormControl<number>;

	day1night: FormControl<number>;
	day2night: FormControl<number>;
	day3night: FormControl<number>;
	day4night: FormControl<number>;
	day5night: FormControl<number>;
	day6night: FormControl<number>;
	day7night: FormControl<number>;
	day8night: FormControl<number>;
	day9night: FormControl<number>;
	day10night: FormControl<number>;
	day11night: FormControl<number>;
	day12night: FormControl<number>;
	day13night: FormControl<number>;
	day14night: FormControl<number>;
	day15night: FormControl<number>;
	day16night: FormControl<number>;
	day17night: FormControl<number>;
	day18night: FormControl<number>;
	day19night: FormControl<number>;
	day20night: FormControl<number>;
	day21night: FormControl<number>;
	day22night: FormControl<number>;
	day23night: FormControl<number>;
	day24night: FormControl<number>;
	day25night: FormControl<number>;
	day26night: FormControl<number>;
	day27night: FormControl<number>;
	day28night: FormControl<number>;
	day29night: FormControl<number>;
	day30night: FormControl<number>;
	day31night: FormControl<number>;
}

export interface WorkingTimeRecordReportHoursFormGroup {
	year: FormControl<number>;
	month: FormControl<number>;
	employeeId: FormControl<number>;
	day1Entry: FormControl<number>;
	day2Entry: FormControl<number>;
	day3Entry: FormControl<number>;
	day4Entry: FormControl<number>;
	day5Entry: FormControl<number>;
	day6Entry: FormControl<number>;
	day7Entry: FormControl<number>;
	day8Entry: FormControl<number>;
	day9Entry: FormControl<number>;
	day10Entry: FormControl<number>;
	day11Entry: FormControl<number>;
	day12Entry: FormControl<number>;
	day13Entry: FormControl<number>;
	day14Entry: FormControl<number>;
	day15Entry: FormControl<number>;
	day16Entry: FormControl<number>;
	day17Entry: FormControl<number>;
	day18Entry: FormControl<number>;
	day19Entry: FormControl<number>;
	day20Entry: FormControl<number>;
	day21Entry: FormControl<number>;
	day22Entry: FormControl<number>;
	day23Entry: FormControl<number>;
	day24Entry: FormControl<number>;
	day25Entry: FormControl<number>;
	day26Entry: FormControl<number>;
	day27Entry: FormControl<number>;
	day28Entry: FormControl<number>;
	day29Entry: FormControl<number>;
	day30Entry: FormControl<number>;
	day31Entry: FormControl<number>;

	day1Exit: FormControl<number>;
	day2Exit: FormControl<number>;
	day3Exit: FormControl<number>;
	day4Exit: FormControl<number>;
	day5Exit: FormControl<number>;
	day6Exit: FormControl<number>;
	day7Exit: FormControl<number>;
	day8Exit: FormControl<number>;
	day9Exit: FormControl<number>;
	day10Exit: FormControl<number>;
	day11Exit: FormControl<number>;
	day12Exit: FormControl<number>;
	day13Exit: FormControl<number>;
	day14Exit: FormControl<number>;
	day15Exit: FormControl<number>;
	day16Exit: FormControl<number>;
	day17Exit: FormControl<number>;
	day18Exit: FormControl<number>;
	day19Exit: FormControl<number>;
	day20Exit: FormControl<number>;
	day21Exit: FormControl<number>;
	day22Exit: FormControl<number>;
	day23Exit: FormControl<number>;
	day24Exit: FormControl<number>;
	day25Exit: FormControl<number>;
	day26Exit: FormControl<number>;
	day27Exit: FormControl<number>;
	day28Exit: FormControl<number>;
	day29Exit: FormControl<number>;
	day30Exit: FormControl<number>;
	day31Exit: FormControl<number>;
}
