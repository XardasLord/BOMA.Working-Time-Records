import { AfterViewInit, Component } from '@angular/core';
import { Store } from '@ngxs/store';
import { WorkingTimeRecordState } from '../../state/working-time-record.state';
import { EmployeeWorkingTimeRecordDetailsModel } from '../../models/employee-working-time-record-details.model';
import { WorkingTimeRecordDetailsAggregatedModel } from '../../models/working-time-record-details-aggregated.model';
import { WorkingTimeRecord } from '../../state/working-time-record.action';
import { map } from 'rxjs/operators';
import GetAll = WorkingTimeRecord.GetAll;
import PrintData = WorkingTimeRecord.PrintData;

@Component({
	selector: 'app-working-time-record-report-hours-table',
	templateUrl: './working-time-record-report-hours-table.component.html',
	styleUrls: ['./working-time-record-report-hours-table.component.scss']
})
export class WorkingTimeRecordReportHoursTableComponent implements AfterViewInit {
	detailedRecords$ = this.store.select(WorkingTimeRecordState.getReportHourRecordsNormalizedForTable);
	numberOfDays$ = this.store.select(WorkingTimeRecordState.getDaysInMonth);
	columnsToDisplay$ = this.store.select(WorkingTimeRecordState.getColumnsToDisplayForReportHours);

	constructor(private store: Store) {}

	ngAfterViewInit() {
		this.store.dispatch(new GetAll());
	}

	trackRecord(index: number, element: EmployeeWorkingTimeRecordDetailsModel): number {
		return element.employee.id;
	}

	getAllHoursSum(workingTimeRecordDetails: WorkingTimeRecordDetailsAggregatedModel[]) {
		return workingTimeRecordDetails.reduce<number>((accumulator, obj) => accumulator + obj.workedHoursRounded + obj.saturdayHours, 0);
	}

	getTotalHoursSum() {
		return this.detailedRecords$.pipe(
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

	onPrint(divIdName: string): void {
		this.store.dispatch(new PrintData(divIdName));
	}
}