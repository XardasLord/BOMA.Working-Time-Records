import { AfterViewInit, Component } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { WorkingTimeRecordState } from '../../state/working-time-record.state';
import { Observable } from 'rxjs';
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

	columnsToDisplay = ['fullName', 'rate', 'gross', 'bonus', 'sumValue', 'sumBonus', 'sumHours', 'emptyLabel'];
	daysArray: string[] = [];

	constructor(private store: Store) {
		for (let i = 1; i <= this.daysInMonth(6, 2022); i++) {
			this.columnsToDisplay.push(i.toString());
			this.daysArray.push(i.toString());
		}
	}

	ngAfterViewInit() {
		this.store.dispatch(new GetAll());
	}

	daysInMonth(month: number, year: number) {
		return new Date(year, month, 0).getDate();
	}

	trackDetailedRecord(index: number, element: EmployeeWorkingTimeRecordDetailsModel): number {
		return element.employee.id;
	}

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
