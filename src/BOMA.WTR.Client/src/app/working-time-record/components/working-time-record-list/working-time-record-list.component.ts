import { Component, inject, OnInit } from '@angular/core';
import { WorkingTimeRecord } from '../../state/working-time-record.action';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthState } from '../../../shared/auth/state/auth.state';
import { Role } from '../../../shared/auth/models/userDetails';
import { WorkingTimeRecordDetailedTableComponent } from '../working-time-record-detailed-table/working-time-record-detailed-table.component';
import { WorkingTimeRecordSummaryTableComponent } from '../working-time-record-summary-table/working-time-record-summary-table.component';
import { WorkingTimeRecordReportHoursTableComponent } from '../working-time-record-report-hours-table/working-time-record-report-hours-table.component';
import { WorkingTimeRecordReportHoursSummaryTableComponent } from '../working-time-record-report-hours-summary-table/working-time-record-report-hours-summary-table.component';
import { WorkingTimeRecordReportAbsentsTableComponent } from '../working-time-record-report-absents-table/working-time-record-report-absents-table.component';
import GetAll = WorkingTimeRecord.GetAll;

@Component({
	selector: 'app-working-time-record-list',
	templateUrl: './working-time-record-list.component.html',
	styleUrls: ['./working-time-record-list.component.scss']
})
export class WorkingTimeRecordListComponent implements OnInit {
	private store = inject(Store);

	allTabs: WorkingTimeTab[] = [
		{
			label: 'Szczegóły',
			component: WorkingTimeRecordDetailedTableComponent,
			visibleFor: [Role.Admin]
		},
		{
			label: 'Podsumowanie',
			component: WorkingTimeRecordSummaryTableComponent,
			visibleFor: [Role.Admin]
		},
		{
			label: 'Raport godzinowy',
			component: WorkingTimeRecordReportHoursTableComponent,
			visibleFor: []
		},
		{
			label: 'Raport godzinowy - podsumowanie',
			component: WorkingTimeRecordReportHoursSummaryTableComponent,
			visibleFor: []
		},
		{
			label: 'Raport nieobecności',
			component: WorkingTimeRecordReportAbsentsTableComponent,
			visibleFor: [Role.Admin, Role.User, Role.UserWithSalaryView, Role.UserWithSalaryEdit]
		}
	];

	filteredTabs$!: Observable<WorkingTimeTab[]>;
	Role = Role;

	userRole$ = this.store.select(AuthState.getUserRole);

	ngOnInit(): void {
		this.store.dispatch(new GetAll());

		this.filteredTabs$ = this.userRole$.pipe(
			map((role) => this.allTabs.filter((tab) => tab.visibleFor.length === 0 || tab.visibleFor.includes(role!)))
		);
	}
}

interface WorkingTimeTab {
	label: string;
	component: any;
	visibleFor: Role[];
}
