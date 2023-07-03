import { AfterViewInit, Component } from '@angular/core';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { WorkingTimeRecordState } from '../../state/working-time-record.state';
import { EmployeeWorkingTimeRecordAbsentsModel } from '../../models/employee-working-time-record-absents.model';

@Component({
	selector: 'app-working-time-record-report-absents-table',
	templateUrl: './working-time-record-report-absents-table.component.html',
	styleUrls: ['./working-time-record-report-absents-table.component.scss']
})
export class WorkingTimeRecordReportAbsentsTableComponent implements AfterViewInit {
	detailedRecords$!: Observable<EmployeeWorkingTimeRecordAbsentsModel[]>;
	numberOfDays$ = this.store.select(WorkingTimeRecordState.getDaysInMonth);
	columnsToDisplay$ = this.store.select(WorkingTimeRecordState.getColumnsToDisplayForAbsentReportTable);

	constructor(private store: Store) {}

	ngAfterViewInit() {
		this.detailedRecords$ = this.store.select(WorkingTimeRecordState.getReportAbsentEmployeesRecordsNormalizedForTable);
	}

	trackRecord(index: number, element: EmployeeWorkingTimeRecordAbsentsModel): number {
		return element.employee.id;
	}
}
