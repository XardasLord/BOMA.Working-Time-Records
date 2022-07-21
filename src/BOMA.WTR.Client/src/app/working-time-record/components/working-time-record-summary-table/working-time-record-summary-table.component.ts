import { Component } from '@angular/core';
import { Store } from '@ngxs/store';
import { WorkingTimeRecordState } from '../../state/working-time-record.state';
import { EmployeeWorkingTimeRecordDetailsModel } from '../../models/employee-working-time-record-details.model';

@Component({
	selector: 'app-working-time-record-summary-table',
	templateUrl: './working-time-record-summary-table.component.html',
	styleUrls: ['./working-time-record-summary-table.component.scss']
})
export class WorkingTimeRecordSummaryTableComponent {
	detailedRecords$ = this.store.select(WorkingTimeRecordState.getDetailedRecords);

	columnsToDisplay = [
		'fullName',
		'rate',
		'bonusPercentage',
		'gross',
		'bonus',
		'overtimes',
		'saturdays',
		'night',
		'nightHours',
		'holiday',
		'sickness',
		'additional',
		'sum',
		'actions'
	];

	constructor(private store: Store) {}

	editRecord(record: EmployeeWorkingTimeRecordDetailsModel) {
		if (!record.isEditable) {
			console.log('This record cannot be edited');
		}
	}
}
