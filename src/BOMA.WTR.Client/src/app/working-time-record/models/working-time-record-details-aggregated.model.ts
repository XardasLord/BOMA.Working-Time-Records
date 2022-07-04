import { RecordEventType } from './record-event-type';

export class WorkingTimeRecordDetailsAggregatedModel {
	recordEventType!: RecordEventType;
	date!: Date;
	workedMinutes!: number;
	workedHoursRounded!: number;
}
