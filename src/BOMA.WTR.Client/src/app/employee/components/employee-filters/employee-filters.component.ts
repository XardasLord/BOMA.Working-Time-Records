import { Component, inject } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { MatOption } from '@angular/material/core';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { Store } from '@ngxs/store';

import { DepartmentsArray } from '../../../shared/models/departments-array';
import { DepartmentsEnum } from '../../../shared/models/departments.enum';
import { ShiftTypesArray } from '../../../shared/models/shift-types-array';
import { ShiftTypesEnum } from '../../../shared/models/shift-types.enum';
import { KeyValuePair } from 'src/app/working-time-record/models/key-value-pair.model';
import { EmployeeState } from '../../state/employee.state';
import { SharedModule } from '../../../shared/shared.module';
import { Employee } from '../../state/employee.action';

import ChangeGroup = Employee.ChangeGroup;
import ChangeShift = Employee.ChangeShift;
import ApplyFilter = Employee.ApplyFilter;

@Component({
	selector: 'app-employee-filters',
	templateUrl: './employee-filters.component.html',
	styleUrls: ['./employee-filters.component.scss'],
	standalone: true,
	imports: [MatFormField, MatLabel, MatIcon, MatOption, SharedModule]
})
export class EmployeeFiltersComponent {
	private store = inject(Store);

	departments: KeyValuePair<DepartmentsEnum, number>[] = DepartmentsArray;
	shifts: KeyValuePair<ShiftTypesEnum, number>[] = ShiftTypesArray;

	queryModel$ = this.store.select(EmployeeState.getQueryModel);

	groupChanged(groupId: number): void {
		this.store.dispatch(new ChangeGroup(groupId));
	}

	shiftChanged(shiftId: number): void {
		this.store.dispatch(new ChangeShift(shiftId));
	}

	searchTextChanged(search: string): void {
		this.store.dispatch(new ApplyFilter(search));
	}
}
