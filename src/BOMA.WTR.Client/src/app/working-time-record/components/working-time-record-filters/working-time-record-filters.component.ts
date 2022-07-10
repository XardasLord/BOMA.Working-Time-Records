import { Component, OnInit } from '@angular/core';
import { Store } from '@ngxs/store';
import { WorkingTimeRecord } from '../../state/working-time-record.action';
import GetAll = WorkingTimeRecord.GetAll;

@Component({
	selector: 'app-working-time-record-filters',
	templateUrl: './working-time-record-filters.component.html',
	styleUrls: ['./working-time-record-filters.component.scss']
})
export class WorkingTimeRecordFiltersComponent implements OnInit {
	constructor(private store: Store) {}

	ngOnInit(): void {}

	groupChanged(groupId: number): void {
		const today = new Date();
		const year = today.getFullYear();
		const month = today.getMonth() + 1;

		this.store.dispatch(new GetAll(year, 6, groupId));
	}
}
