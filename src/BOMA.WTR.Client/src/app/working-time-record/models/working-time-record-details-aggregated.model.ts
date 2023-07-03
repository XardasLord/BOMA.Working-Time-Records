export class WorkingTimeRecordDetailsAggregatedModel {
	date!: Date;
	startNormalizedAt!: Date;
	finishNormalizedAt!: Date;
	startOriginalAt!: Date;
	finishOriginalAt!: Date;
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
