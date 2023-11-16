import { FormControl } from '@angular/forms';

export interface WorkingTimeRecordSummaryDataFormModel {
	employeeId: number;
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
