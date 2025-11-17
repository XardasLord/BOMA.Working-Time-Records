import { Component, inject } from '@angular/core';
import { AsyncPipe } from '@angular/common';
import { Observable } from 'rxjs';
import { EmployeeWorkingTimeRecordDetailsModel } from '../../../working-time-record/models/employee-working-time-record-details.model';
import { Store } from '@ngxs/store';
import { ReportsState } from '../../state/reports.state';
import { map } from 'rxjs/operators';

@Component({
	selector: 'app-working-hours-report',
	standalone: true,
	imports: [AsyncPipe],
	templateUrl: './working-hours-report.component.html',
	styleUrl: './working-hours-report.component.scss'
})
export class WorkingHoursReportComponent {
	private store = inject(Store);

	detailedRecords$: Observable<EmployeeWorkingTimeRecordDetailsModel[]> = this.store.select(ReportsState.getDetailedRecords);

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
