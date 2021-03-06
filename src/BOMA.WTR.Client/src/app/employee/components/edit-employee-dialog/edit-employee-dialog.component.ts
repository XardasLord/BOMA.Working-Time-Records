import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngxs/store';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ResetForm, UpdateFormValue } from '@ngxs/form-plugin';
import { EmployeeModel } from '../../../shared/models/employee.model';
import { Employee } from '../../state/employee.action';
import Edit = Employee.Edit;
import { DepartmentsArray } from '../../../shared/models/departments-array';

@Component({
	selector: 'app-edit-employee-dialog',
	templateUrl: './edit-employee-dialog.component.html',
	styleUrls: ['./edit-employee-dialog.component.scss']
})
export class EditEmployeeDialogComponent implements OnInit {
	editEmployeeForm!: FormGroup;
	departments = DepartmentsArray;

	constructor(fb: FormBuilder, private store: Store, @Inject(MAT_DIALOG_DATA) employee: EmployeeModel) {
		this.editEmployeeForm = fb.group({
			id: new FormControl<number>(employee.id, [Validators.required]),
			firstName: new FormControl<string>(employee.firstName, [Validators.required, Validators.max(64)]),
			lastName: new FormControl<string>(employee.lastName, [Validators.required, Validators.max(64)]),
			baseSalary: new FormControl<number>(employee.baseSalary, [Validators.required, Validators.min(0)]),
			salaryBonusPercentage: new FormControl<number>(employee.salaryBonusPercentage, [Validators.required, Validators.min(0)]),
			rcpId: new FormControl<number>(employee.rcpId, [Validators.required, Validators.min(1)]),
			departmentId: new FormControl<number>(employee.departmentId, [Validators.required]),
			departmentName: new FormControl<string>(employee.departmentName)
		});

		this.store.dispatch(new ResetForm({ path: 'employee.editEmployeeForm', value: this.editEmployeeForm.value }));
	}

	ngOnInit(): void {}

	onSubmit() {
		if (!this.editEmployeeForm.valid) return;

		this.store.dispatch(
			new UpdateFormValue({
				path: 'employee.editEmployeeForm',
				value: this.departments.filter((x) => x.value === this.editEmployeeForm.value.departmentId)[0].key,
				propertyPath: 'departmentName'
			})
		);

		this.store.dispatch(new Edit(this.editEmployeeForm.value.id, this.editEmployeeForm.value));
	}
}
