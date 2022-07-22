import { WorkingTimeRecordSummaryDataFormModel } from '../models/working-time-record-summary-data-form.model';

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

	export class ChangeDate {
		constructor(public year: number, public month: number) {}

		static readonly type = `${prefix} ${ChangeDate.name}`;
	}

	export class UpdateSummaryData {
		constructor(public employeeId: number, public updateModel: WorkingTimeRecordSummaryDataFormModel) {}

		static readonly type = `${prefix} ${UpdateSummaryData.name}`;
	}
}
