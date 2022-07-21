import { AfterViewInit, Component } from '@angular/core';
import { Store } from '@ngxs/store';
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
	detailedRecords$ = this.store.select(WorkingTimeRecordState.getDetailedRecordsNormalizedForTable);
	numberOfDays$ = this.store.select(WorkingTimeRecordState.getDaysInMonth);
	columnsToDisplay$ = this.store.select(WorkingTimeRecordState.getColumnsToDisplay);

	constructor(private store: Store) {}

	ngAfterViewInit() {
		this.store.dispatch(new GetAll());
	}

	trackDetailedRecord(index: number, element: EmployeeWorkingTimeRecordDetailsModel): number {
		return element.employee.id;
	}

	editRecord(record: EmployeeWorkingTimeRecordDetailsModel) {}

	// isWeekendDay(workingTimeRecordDetails: WorkingTimeRecordDetailsAggregatedModel[], dayOfMonth: number): boolean {
	// 	const recordFromDay = workingTimeRecordDetails.filter((x) => new Date(x.date).getDate() === dayOfMonth);
	// 	return recordFromDay[0]?.isWeekendDay ?? true;
	// }

	getWorkingHoursSum(workingTimeRecordDetails: WorkingTimeRecordDetailsAggregatedModel[]) {
		return workingTimeRecordDetails.reduce<number>((accumulator, obj) => accumulator + obj.workedHoursRounded, 0);
	}

	getNormativeHoursSum(workingTimeRecordDetails: WorkingTimeRecordDetailsAggregatedModel[]) {
		return workingTimeRecordDetails.reduce<number>((accumulator, obj) => accumulator + obj.baseNormativeHours, 0);
	}

	get50PercentageHoursSum(workingTimeRecordDetails: WorkingTimeRecordDetailsAggregatedModel[]) {
		return workingTimeRecordDetails.reduce<number>((accumulator, obj) => accumulator + obj.fiftyPercentageBonusHours, 0);
	}

	get100PercentageHoursSum(workingTimeRecordDetails: WorkingTimeRecordDetailsAggregatedModel[]) {
		return workingTimeRecordDetails.reduce<number>((accumulator, obj) => accumulator + obj.hundredPercentageBonusHours, 0);
	}

	getSaturdayHoursSum(workingTimeRecordDetails: WorkingTimeRecordDetailsAggregatedModel[]) {
		return workingTimeRecordDetails.reduce<number>((accumulator, obj) => accumulator + obj.saturdayHours, 0);
	}

	getNightHoursSum(workingTimeRecordDetails: WorkingTimeRecordDetailsAggregatedModel[]) {
		return workingTimeRecordDetails.reduce<number>((accumulator, obj) => accumulator + obj.nightHours, 0);
	}
}
