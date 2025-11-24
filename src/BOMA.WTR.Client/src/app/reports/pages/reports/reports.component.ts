import { Component, inject, OnInit } from '@angular/core';
import { ReportFiltersComponent } from '../../components/report-filters/report-filters.component';
import { Store } from '@ngxs/store';
import { WorkingHoursReportComponent } from '../../components/working-hours-report/working-hours-report.component';
import { SalaryReportComponent } from '../../components/salary-report/salary-report.component';
import { Reports } from '../../state/reports.action';
import LoadData = Reports.LoadData;
import { MatFabButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatTooltip } from '@angular/material/tooltip';
import { NgIf } from '@angular/common';
import { NgxPrintDirective } from 'ngx-print';

@Component({
	selector: 'app-reports',
	standalone: true,
	imports: [
		ReportFiltersComponent,
		WorkingHoursReportComponent,
		SalaryReportComponent,
		MatFabButton,
		MatIcon,
		MatTooltip,
		NgIf,
		NgxPrintDirective
	],
	templateUrl: './reports.component.html',
	styleUrl: './reports.component.scss'
})
export class ReportsComponent implements OnInit {
	private store = inject(Store);

	ngOnInit(): void {
		this.store.dispatch(new LoadData());
	}
}
