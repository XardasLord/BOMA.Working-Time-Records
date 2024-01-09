import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngxs/store';
import { UpdateFormValue } from '@ngxs/form-plugin';
import { DepartmentsArray } from '../../../shared/models/departments-array';
import { ShiftTypesArray } from '../../../shared/models/shift-types-array';
import { Employee } from '../../state/employee.action';
import Add = Employee.Add;
import { AddNewEmployeeFormGroup } from '../../models/add-new-employee-form-group';
import { AddNewEmployeeFormModel } from '../../models/add-new-employee-form.model';
import { nameof } from '../../../shared/helpers/name-of.helper';

@Component({
	selector: 'app-add-new-employee-dialog',
	templateUrl: './add-new-employee-dialog.component.html',
	styleUrls: ['./add-new-employee-dialog.component.scss']
})
export class AddNewEmployeeDialogComponent {
	addNewEmployeeForm!: FormGroup<AddNewEmployeeFormGroup>;

	departments = DepartmentsArray;
	shiftTypes = ShiftTypesArray;

	constructor(
		fb: FormBuilder,
		private store: Store
	) {
		this.addNewEmployeeForm = fb.group<AddNewEmployeeFormGroup>({
			firstName: new FormControl<string>('', { nonNullable: true, validators: [Validators.required, Validators.max(64)] }),
			lastName: new FormControl<string>('', { nonNullable: true, validators: [Validators.required, Validators.max(64)] }),
			baseSalary: new FormControl<number>(0, { nonNullable: true, validators: [Validators.required, Validators.min(0)] }),
			salaryBonusPercentage: new FormControl<number>(0, { nonNullable: true, validators: [Validators.required, Validators.min(0)] }),
			rcpId: new FormControl<number>(0, { nonNullable: true, validators: [Validators.required, Validators.min(1)] }),
			departmentId: new FormControl<number>(0, { nonNullable: true, validators: [Validators.required] }),
			departmentName: new FormControl<string>(''),
			shiftTypeId: new FormControl<number | null>(0, { nonNullable: true, validators: [Validators.required] }),
			shiftTypeName: new FormControl<string | null>(''),
			position: new FormControl<string>('', { nonNullable: true, validators: [Validators.max(64)] })
		});
	}

	onSubmit() {
		if (!this.addNewEmployeeForm.valid) return;

		this.store.dispatch(
			new UpdateFormValue({
				path: 'employee.addNewEmployeeForm',
				value: this.departments.filter((x) => x.value === this.addNewEmployeeForm.value.departmentId)[0].key,
				propertyPath: nameof<AddNewEmployeeFormModel>('departmentName')
			})
		);

		this.store.dispatch(
			new UpdateFormValue({
				path: 'employee.addNewEmployeeForm',
				value: this.shiftTypes.filter((x) => x.value === this.addNewEmployeeForm.value.shiftTypeId)[0].key,
				propertyPath: nameof<AddNewEmployeeFormModel>('shiftTypeName')
			})
		);

		this.store.dispatch(new Add(this.addNewEmployeeForm.value as AddNewEmployeeFormModel));
	}
}
