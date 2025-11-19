import { Component } from '@angular/core';
import { AsyncPipe, NgForOf } from '@angular/common';
import { MatFormField, MatLabel, MatPrefix } from '@angular/material/form-field';
import { MatIcon } from '@angular/material/icon';
import { MatInput } from '@angular/material/input';
import { MatOption } from '@angular/material/core';
import { MatSelect } from '@angular/material/select';
import { Store } from '@ngxs/store';
import { SharedModule } from '../../../shared/shared.module';
import { KeyValuePair } from '../../../working-time-record/models/key-value-pair.model';
import { DepartmentsEnum } from '../../../shared/models/departments.enum';
import { DepartmentsArray } from '../../../shared/models/departments-array';
import { ShiftTypesEnum } from '../../../shared/models/shift-types.enum';
import { ShiftTypesArray } from '../../../shared/models/shift-types-array';
import { Reports } from '../../state/reports.action';
import { ReportsState } from '../../state/reports.state';
import { MonthRangePickerComponent, MonthRange } from '../month-range-picker/month-range-picker.component';
import ChangeGroup = Reports.ChangeGroup;
import ChangeShift = Reports.ChangeShift;
import ApplyFilter = Reports.ApplyFilter;
import ChangeDateRange = Reports.ChangeDateRange;

@Component({
	selector: 'app-report-filters',
	standalone: true,
	imports: [
		AsyncPipe,
		MatFormField,
		MatIcon,
		MatInput,
		MatLabel,
		MatOption,
		MatPrefix,
		MatSelect,
		NgForOf,
		SharedModule,
		MonthRangePickerComponent
	],
	templateUrl: './report-filters.component.html',
	styleUrl: './report-filters.component.scss'
})
export class ReportFiltersComponent {
	departments: KeyValuePair<DepartmentsEnum, number>[] = DepartmentsArray;
	shifts: KeyValuePair<ShiftTypesEnum, number>[] = ShiftTypesArray;

	queryModel$ = this.store.select(ReportsState.getSearchQueryModel);

	constructor(private store: Store) {}

	groupChanged(groupId: number): void {
		this.store.dispatch(new ChangeGroup(groupId));
	}

	shiftChanged(shiftId: number): void {
		this.store.dispatch(new ChangeShift(shiftId));
	}

	searchTextChanged(search: string): void {
		this.store.dispatch(new ApplyFilter(search));
	}

	dateRangeChanged(range: MonthRange): void {
		this.store.dispatch(new ChangeDateRange(range.start, range.end));
	}
}
