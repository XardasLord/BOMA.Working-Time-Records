import { Component, inject } from '@angular/core';
import { AsyncPipe, CurrencyPipe } from '@angular/common';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { EmployeeWorkingTimeRecordDetailsModel } from '../../../working-time-record/models/employee-working-time-record-details.model';
import { ReportsState } from '../../state/reports.state';
import { map } from 'rxjs/operators';

@Component({
	selector: 'app-salary-report',
	standalone: true,
	imports: [AsyncPipe, CurrencyPipe],
	templateUrl: './salary-report.component.html',
	styleUrl: './salary-report.component.scss'
})
export class SalaryReportComponent {
	private store = inject(Store);

	detailedRecords$: Observable<EmployeeWorkingTimeRecordDetailsModel[]> = this.store.select(ReportsState.getDetailedRecords);

	getTotalNumberOfEmployees(): Observable<number> {
		return this.detailedRecords$?.pipe(map((results) => results.length));
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
