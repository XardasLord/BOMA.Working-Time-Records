import { AfterViewInit, Component } from '@angular/core';
import { Store } from '@ngxs/store';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { WorkingTimeRecordState } from '../../state/working-time-record.state';
import { EmployeeWorkingTimeRecordDetailsModel } from '../../models/employee-working-time-record-details.model';

import { Role } from '../../../shared/auth/models/userDetails';

@Component({
	selector: 'app-working-time-record-reports',
	templateUrl: './working-time-record-reports.component.html',
	styleUrls: ['./working-time-record-reports.component.scss']
})
export class WorkingTimeRecordReportsComponent implements AfterViewInit {
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

	getTotalGrossBaseSalarySum(): Observable<number> {
		return this.detailedRecords$?.pipe(
			map((results) => results.map((r) => r.salaryInformation.grossBaseSalary).reduce((acc, obj) => acc + obj, 0))
		);
	}

	getTotalBonusSalarySum(): Observable<number> {
		return this.detailedRecords$?.pipe(
			map((results) =>
				results
					.map(
						(r) =>
							r.salaryInformation.bonusBaseSalary +
							r.salaryInformation.bonusBase50PercentageSalary +
							r.salaryInformation.bonusBase100PercentageSalary +
							r.salaryInformation.bonusBaseSaturdaySalary
					)
					.reduce((acc, obj) => acc + obj, 0)
			)
		);
	}

	getTotalOvertimeSalarySum(): Observable<number> {
		return this.detailedRecords$?.pipe(
			map((results) =>
				results
					.map((r) => r.salaryInformation.grossBase50PercentageSalary + r.salaryInformation.grossBase100PercentageSalary)
					.reduce((acc, obj) => acc + obj, 0)
			)
		);
	}

	getTotalSaturdaySalarySum(): Observable<number> {
		return this.detailedRecords$?.pipe(
			map((results) =>
				results
					.map((r) => r.salaryInformation.grossBaseSaturdaySalary + r.salaryInformation.grossBase100PercentageSalary)
					.reduce((acc, obj) => acc + obj, 0)
			)
		);
	}

	getTotalNightSalarySum(): Observable<number> {
		return this.detailedRecords$?.pipe(
			map((results) => results.map((r) => r.salaryInformation.nightBaseSalary).reduce((acc, obj) => acc + obj, 0))
		);
	}

	getTotalHolidaySalarySum(): Observable<number> {
		return this.detailedRecords$?.pipe(
			map((results) => results.map((r) => r.salaryInformation.holidaySalary).reduce((acc, obj) => acc + obj, 0))
		);
	}

	getTotalSicknessSalarySum(): Observable<number> {
		return this.detailedRecords$?.pipe(
			map((results) => results.map((r) => r.salaryInformation.sicknessSalary).reduce((acc, obj) => acc + obj, 0))
		);
	}

	getTotalAdditionalSalarySum(): Observable<number> {
		return this.detailedRecords$?.pipe(
			map((results) => results.map((r) => r.salaryInformation.additionalSalary).reduce((acc, obj) => acc + obj, 0))
		);
	}

	getTotalCompensationSalarySum(): Observable<number> {
		return this.detailedRecords$?.pipe(
			map((results) => results.map((r) => r.salaryInformation.minSalaryCompensationAmount).reduce((acc, obj) => acc + obj, 0))
		);
	}

	getTotalFinalSalarySum(): Observable<number> {
		return this.detailedRecords$?.pipe(
			map((results) => results.map((r) => r.salaryInformation.finalSumSalary).reduce((acc, obj) => acc + obj, 0))
		);
	}
}
