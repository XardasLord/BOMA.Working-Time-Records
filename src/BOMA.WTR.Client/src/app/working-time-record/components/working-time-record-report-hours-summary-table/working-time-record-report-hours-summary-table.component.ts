import { AfterViewInit, Component } from '@angular/core';
import { Store } from '@ngxs/store';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { WorkingTimeRecordState } from '../../state/working-time-record.state';
import { EmployeeWorkingTimeRecordDetailsModel } from '../../models/employee-working-time-record-details.model';

import { Role } from '../../../shared/auth/models/userDetails';

@Component({
	selector: 'app-working-time-record-report-hours-summary-table',
	templateUrl: './working-time-record-report-hours-summary-table.component.html',
	styleUrls: ['./working-time-record-report-hours-summary-table.component.scss']
})
export class WorkingTimeRecordReportHoursSummaryTableComponent implements AfterViewInit {
	protected readonly Role = Role;

	detailedRecords$!: Observable<EmployeeWorkingTimeRecordDetailsModel[]>;

	constructor(private store: Store) {}

	ngAfterViewInit() {
		this.detailedRecords$ = this.store.select(WorkingTimeRecordState.getDetailedRecords);
	}

	getTotalNumberOfEmployees(): Observable<number> {
		return this.detailedRecords$?.pipe(map((results) => results.length));
	}

	getTotalNormativeHoursSum(): Observable<number> {
		return this.detailedRecords$?.pipe(
			map((results) =>
				results
					.map((r) => r.workingTimeRecordsAggregated.reduce((accumulator, obj) => accumulator + obj.baseNormativeHours, 0))
					.reduce((acc, obj) => acc + obj, 0)
			)
		);
	}

	getTotalFiftyPercentageBonusHoursSum(): Observable<number> {
		return this.detailedRecords$?.pipe(
			map((results) =>
				results
					.map((r) => r.workingTimeRecordsAggregated.reduce((accumulator, obj) => accumulator + obj.fiftyPercentageBonusHours, 0))
					.reduce((acc, obj) => acc + obj, 0)
			)
		);
	}

	getTotalHundredPercentageBonusHoursSum(): Observable<number> {
		return this.detailedRecords$?.pipe(
			map((results) =>
				results
					.map((r) =>
						r.workingTimeRecordsAggregated.reduce((accumulator, obj) => accumulator + obj.hundredPercentageBonusHours, 0)
					)
					.reduce((acc, obj) => acc + obj, 0)
			)
		);
	}

	getTotalSaturdayHoursSum(): Observable<number> {
		return this.detailedRecords$?.pipe(
			map((results) =>
				results
					.map((r) => r.workingTimeRecordsAggregated.reduce((accumulator, obj) => accumulator + obj.saturdayHours, 0))
					.reduce((acc, obj) => acc + obj, 0)
			)
		);
	}
}
