import { AfterViewInit, Component } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { WorkingTimeRecordState } from '../../state/working-time-record.state';
import { EmployeeWorkingTimeRecordDetailsModel } from '../../models/employee-working-time-record-details.model';
import { WorkingTimeRecordDetailsAggregatedModel } from '../../models/working-time-record-details-aggregated.model';
import { WorkingTimeRecord } from '../../state/working-time-record.action';
import GetAll = WorkingTimeRecord.GetAll;

@Component({
	selector: 'app-working-time-record-detailed-table',
	templateUrl: './working-time-record-detailed-table.component.html',
	styleUrls: ['./working-time-record-detailed-table.component.scss']
})
export class WorkingTimeRecordDetailedTableComponent implements AfterViewInit {
	@Select(WorkingTimeRecordState.getDetailedRecordsNormalizedForTable) detailedRecords$!: Observable<
		EmployeeWorkingTimeRecordDetailsModel[]
	>;
	@Select(WorkingTimeRecordState.getDaysInMonth) numberOfDays$!: Observable<string[]>;
	@Select(WorkingTimeRecordState.getColumnsToDisplay) columnsToDisplay$!: Observable<string[]>;

	constructor(private store: Store) {}

	ngAfterViewInit() {
		this.store.dispatch(new GetAll());
	}

	trackDetailedRecord(index: number, element: EmployeeWorkingTimeRecordDetailsModel): number {
		return element.employee.id;
	}

	// isWeekendDay(workingTimeRecordDetails: WorkingTimeRecordDetailsAggregatedModel[], dayOfMonth: number): boolean {
	// 	const recordFromDay = workingTimeRecordDetails.filter((x) => new Date(x.date).getDate() === dayOfMonth);
	// 	return recordFromDay[0]?.isWeekendDay ?? true;
	// }

	getWorkingHoursForDay(workingTimeRecordDetails: WorkingTimeRecordDetailsAggregatedModel[], dayOfMonth: number) {
		const recordsFromDay = workingTimeRecordDetails.filter((x) => new Date(x.date).getDate() === dayOfMonth);

		if (recordsFromDay.length === 0) {
			return 0;
		}

		return recordsFromDay.reduce<number>((accumulator, obj) => accumulator + obj.workedHoursRounded, 0);
	}

	getWorkingHoursSum(workingTimeRecordDetails: WorkingTimeRecordDetailsAggregatedModel[]) {
		return workingTimeRecordDetails.reduce<number>((accumulator, obj) => accumulator + obj.workedHoursRounded, 0);
	}

	getNormativeHoursForDay(workingTimeRecordDetails: WorkingTimeRecordDetailsAggregatedModel[], dayOfMonth: number) {
		const recordsFromDay = workingTimeRecordDetails.filter((x) => new Date(x.date).getDate() === dayOfMonth);

		if (recordsFromDay.length === 0) {
			return 0;
		}

		return recordsFromDay.reduce<number>((accumulator, obj) => accumulator + obj.baseNormativeHours, 0);
	}

	getNormativeHoursSum(workingTimeRecordDetails: WorkingTimeRecordDetailsAggregatedModel[]) {
		return workingTimeRecordDetails.reduce<number>((accumulator, obj) => accumulator + obj.baseNormativeHours, 0);
	}

	get50PercentageHoursForDay(workingTimeRecordDetails: WorkingTimeRecordDetailsAggregatedModel[], dayOfMonth: number) {
		const recordsFromDay = workingTimeRecordDetails.filter((x) => new Date(x.date).getDate() === dayOfMonth);

		if (recordsFromDay.length === 0) {
			return 0;
		}

		return recordsFromDay.reduce<number>((accumulator, obj) => accumulator + obj.fiftyPercentageBonusHours, 0);
	}

	get50PercentageHoursSum(workingTimeRecordDetails: WorkingTimeRecordDetailsAggregatedModel[]) {
		return workingTimeRecordDetails.reduce<number>((accumulator, obj) => accumulator + obj.fiftyPercentageBonusHours, 0);
	}

	get100PercentageHoursForDay(workingTimeRecordDetails: WorkingTimeRecordDetailsAggregatedModel[], dayOfMonth: number) {
		const recordsFromDay = workingTimeRecordDetails.filter((x) => new Date(x.date).getDate() === dayOfMonth);

		if (recordsFromDay.length === 0) {
			return 0;
		}

		return recordsFromDay.reduce<number>((accumulator, obj) => accumulator + obj.hundredPercentageBonusHours, 0);
	}

	get100PercentageHoursSum(workingTimeRecordDetails: WorkingTimeRecordDetailsAggregatedModel[]) {
		return workingTimeRecordDetails.reduce<number>((accumulator, obj) => accumulator + obj.hundredPercentageBonusHours, 0);
	}

	getSaturdayHoursForDay(workingTimeRecordDetails: WorkingTimeRecordDetailsAggregatedModel[], dayOfMonth: number) {
		const recordsFromDay = workingTimeRecordDetails.filter((x) => new Date(x.date).getDate() === dayOfMonth);

		if (recordsFromDay.length === 0) {
			return 0;
		}

		return recordsFromDay.reduce<number>((accumulator, obj) => accumulator + obj.saturdayHours, 0);
	}

	getSaturdayHoursSum(workingTimeRecordDetails: WorkingTimeRecordDetailsAggregatedModel[]) {
		return workingTimeRecordDetails.reduce<number>((accumulator, obj) => accumulator + obj.saturdayHours, 0);
	}

	getNightHoursForDay(workingTimeRecordDetails: WorkingTimeRecordDetailsAggregatedModel[], dayOfMonth: number) {
		const recordsFromDay = workingTimeRecordDetails.filter((x) => new Date(x.date).getDate() === dayOfMonth);

		if (recordsFromDay.length === 0) {
			return 0;
		}

		return recordsFromDay.reduce<number>((accumulator, obj) => accumulator + obj.nightHours, 0);
	}

	getNightHoursSum(workingTimeRecordDetails: WorkingTimeRecordDetailsAggregatedModel[]) {
		return workingTimeRecordDetails.reduce<number>((accumulator, obj) => accumulator + obj.nightHours, 0);
	}
}
