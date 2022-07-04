import { Component, OnInit } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { WorkingTimeRecordState } from '../../state/working-time-record.state';
import { Observable } from 'rxjs';
import { EmployeeWorkingTimeRecordDetailsModel } from '../../models/employee-working-time-record-details.model';
import { WorkingTimeRecord } from '../../state/working-time-record.action';
import GetAll = WorkingTimeRecord.GetAll;

@Component({
	selector: 'app-working-time-record-list',
	templateUrl: './working-time-record-list.component.html',
	styleUrls: ['./working-time-record-list.component.scss']
})
export class WorkingTimeRecordListComponent implements OnInit {
	@Select(WorkingTimeRecordState.getDetailedRecords) detailedRecords$!: Observable<EmployeeWorkingTimeRecordDetailsModel[]>;

	constructor(private store: Store) {}

	ngOnInit(): void {
		this.store.dispatch(new GetAll(2022, 6, 4));
	}
}
