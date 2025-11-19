import { Component, inject } from '@angular/core';
import { AsyncPipe } from '@angular/common';
import { Observable } from 'rxjs';
import { Store } from '@ngxs/store';
import { ReportsState } from '../../state/reports.state';
import { ReportDataModel } from '../../models/report-data.model';

@Component({
	selector: 'app-working-hours-report',
	standalone: true,
	imports: [AsyncPipe],
	templateUrl: './working-hours-report.component.html',
	styleUrl: './working-hours-report.component.scss'
})
export class WorkingHoursReportComponent {
	private store = inject(Store);

	reportData$: Observable<ReportDataModel | null> = this.store.select(ReportsState.getReportData);
}
