import { Component } from '@angular/core';
import { Store } from '@ngxs/store';
import * as moment from 'moment';

import { MonthsArray } from '../../../shared/models/months-array';
import { DepartmentsArray } from '../../../shared/models/departments-array';
import { DepartmentsEnum } from '../../../shared/models/departments.enum';
import { KeyValuePair } from '../../models/key-value-pair.model';
import { WorkingTimeRecordState } from '../../state/working-time-record.state';
import { WorkingTimeRecord } from '../../state/working-time-record.action';
import ChangeDate = WorkingTimeRecord.ChangeDate;
import ApplyFilter = WorkingTimeRecord.ApplyFilter;
import ChangeGroup = WorkingTimeRecord.ChangeGroup;

@Component({
	selector: 'app-working-time-record-filters',
	templateUrl: './working-time-record-filters.component.html',
	styleUrls: ['./working-time-record-filters.component.scss']
})
export class WorkingTimeRecordFiltersComponent {
	years: number[] = [];
	months: KeyValuePair<string, number>[] = MonthsArray;
	departments: KeyValuePair<DepartmentsEnum, number>[] = DepartmentsArray;

	queryModel$ = this.store.select(WorkingTimeRecordState.getSearchQueryModel);

	constructor(private store: Store) {
		const currentYear = moment().year();

		for (let i = currentYear - 5; i <= currentYear; i++) {
			this.years.push(i);
		}
	}

	groupChanged(groupId: number): void {
		this.store.dispatch(new ChangeGroup(groupId));
	}

	searchTextChanged(search: string): void {
		this.store.dispatch(new ApplyFilter(search));
	}

	yearChanged(year: number): void {
		this.store.dispatch(new ChangeDate(year, this.store.selectSnapshot(WorkingTimeRecordState.getSearchQueryModel).month));
	}

	monthChanged(month: number): void {
		this.store.dispatch(new ChangeDate(this.store.selectSnapshot(WorkingTimeRecordState.getSearchQueryModel).year, month));
	}
}
