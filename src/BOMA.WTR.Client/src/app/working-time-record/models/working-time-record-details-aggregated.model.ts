export class WorkingTimeRecordDetailsAggregatedModel {
	date!: Date;
	workTimePeriodNormalized!: WorkTimePeriod;
	workTimePeriodOriginal!: WorkTimePeriod;
	workedMinutes!: number;
	workedHoursRounded!: number;
	baseNormativeHours!: number;
	fiftyPercentageBonusHours!: number;
	hundredPercentageBonusHours!: number;
	saturdayHours!: number;
	nightHours!: number;
	isWeekendDay!: boolean;
	missingRecordEventType?: number;
}

export class WorkingTimeRecordAbsentAggregatedModel {
	date!: Date;
	isWeekendDay!: boolean;
	missingRecordEventType?: number;
	isAbsent!: boolean;
}

export interface WorkTimePeriod {
	from: Date;
	to: Date | null;
}
