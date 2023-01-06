import { Component, OnInit } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { WorkingTimeRecord } from '../../state/working-time-record.action';
import GetAll = WorkingTimeRecord.GetAll;
import { Store } from '@ngxs/store';

@Component({
	selector: 'app-working-time-record-list',
	templateUrl: './working-time-record-list.component.html',
	styleUrls: ['./working-time-record-list.component.scss']
})
export class WorkingTimeRecordListComponent implements OnInit {
	managerMode = environment.managerMode;

	constructor(private store: Store) {}

	ngOnInit(): void {
		this.store.dispatch(new GetAll());
	}
}
