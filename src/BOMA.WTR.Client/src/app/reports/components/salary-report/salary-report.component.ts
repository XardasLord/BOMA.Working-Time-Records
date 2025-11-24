import { Component, inject } from '@angular/core';
import { AsyncPipe, CurrencyPipe, DecimalPipe } from '@angular/common';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { ReportsState } from '../../state/reports.state';
import { ReportDataModel } from '../../models/report-data.model';

@Component({
	selector: 'app-salary-report',
	standalone: true,
	imports: [AsyncPipe, CurrencyPipe, DecimalPipe],
	templateUrl: './salary-report.component.html',
	styleUrl: './salary-report.component.scss'
})
export class SalaryReportComponent {
	private store = inject(Store);

	reportData$: Observable<ReportDataModel | null> = this.store.select(ReportsState.getReportData);

	calculatePercentage(value: number | undefined, total: number | undefined): number {
		if (!value || !total || total === 0) {
			return 0;
		}
		return (value / total) * 100;
	}
}
