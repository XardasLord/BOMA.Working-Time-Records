import { AfterViewInit, Component } from '@angular/core';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { WorkingTimeRecordState } from '../../state/working-time-record.state';
import { EmployeeWorkingTimeRecordAbsentsModel } from '../../models/employee-working-time-record-absents.model';
import { WorkingTimeRecordDetailsAggregatedModel } from '../../models/working-time-record-details-aggregated.model';

@Component({
	selector: 'app-working-time-record-report-absents-table',
	templateUrl: './working-time-record-report-absents-table.component.html',
	styleUrls: ['./working-time-record-report-absents-table.component.scss']
})
export class WorkingTimeRecordReportAbsentsTableComponent implements AfterViewInit {
	detailedRecords$!: Observable<EmployeeWorkingTimeRecordAbsentsModel[]>;
	numberOfDays$ = this.store.select(WorkingTimeRecordState.getDaysInMonth);
	columnsToDisplay$ = this.store.select(WorkingTimeRecordState.getColumnsToDisplayForReportHours);

	constructor(private store: Store) {}

	ngAfterViewInit() {
		this.detailedRecords$ = this.store.select(WorkingTimeRecordState.getReportAbsentEmployeesRecordsNormalizedForTable);
	}

	trackRecord(index: number, element: EmployeeWorkingTimeRecordAbsentsModel): number {
		return element.employee.id;
	}

	getAllHoursSum(workingTimeRecordDetails: WorkingTimeRecordDetailsAggregatedModel[]) {
		return workingTimeRecordDetails.reduce<number>((accumulator, obj) => accumulator + obj.workedHoursRounded + obj.saturdayHours, 0);
	}

	getTotalHoursSum() {
		return this.detailedRecords$?.pipe(
			map(
				(results) =>
					results
						.map((r) =>
							r.workingTimeRecordsAggregated.reduce(
								(accumulator, obj) => accumulator + obj.workedHoursRounded + obj.saturdayHours,
								0
							)
						)
						.reduce((acc, obj) => acc + obj, 0) / 3
			)
		);
	}
}
