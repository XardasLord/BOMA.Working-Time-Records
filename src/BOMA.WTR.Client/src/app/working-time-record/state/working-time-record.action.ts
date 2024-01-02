import {
	WorkingTimeRecordDetailedDataFormModel,
	WorkingTimeRecordReportHoursDataFormModel,
	WorkingTimeRecordSummaryDataFormModel
} from '../models/working-time-record-summary-data-form.model';

export namespace WorkingTimeRecord {
	const prefix = '[WorkingTimeRecord]';

	export class GetAll {
		constructor() {}

		static readonly type = `${prefix} ${GetAll.name}`;
	}

	export class ApplyFilter {
		constructor(public searchPhrase: string) {}

		static readonly type = `${prefix} ${ApplyFilter.name}`;
	}

	export class ChangeGroup {
		constructor(public groupId: number) {}

		static readonly type = `${prefix} ${ChangeGroup.name}`;
	}

	export class ChangeShift {
		constructor(public shiftId: number) {}

		static readonly type = `${prefix} ${ChangeShift.name}`;
	}

	export class ChangeDate {
		constructor(public year: number, public month: number) {}

		static readonly type = `${prefix} ${ChangeDate.name}`;
	}

	export class UpdateSummaryData {
		constructor(public employeeId: number, public updateModel: WorkingTimeRecordSummaryDataFormModel) {}

		static readonly type = `${prefix} ${UpdateSummaryData.name}`;
	}

	export class UpdateDetailedData {
		constructor(public employeeId: number, public updateModel: WorkingTimeRecordDetailedDataFormModel) {}

		static readonly type = `${prefix} ${UpdateDetailedData.name}`;
	}

	export class UpdateReportHoursData {
		constructor(public employeeId: number, public updateModel: WorkingTimeRecordReportHoursDataFormModel) {}

		static readonly type = `${prefix} ${UpdateReportHoursData.name}`;
	}

	export class SendHoursToGratyfikant {
		constructor() {}

		static readonly type = `${prefix} ${SendHoursToGratyfikant.name}`;
	}
}
