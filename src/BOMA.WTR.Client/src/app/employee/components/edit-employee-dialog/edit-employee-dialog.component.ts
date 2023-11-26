import { Component, Inject } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Store } from '@ngxs/store';
import { ResetForm, UpdateFormValue } from '@ngxs/form-plugin';
import { EmployeeModel } from '../../../shared/models/employee.model';
import { Employee } from '../../state/employee.action';
import Edit = Employee.Edit;
import { DepartmentsArray } from '../../../shared/models/departments-array';
import { ShiftTypesArray } from '../../../shared/models/shift-types-array';
import { EditEmployeeFormGroup } from '../../models/edit-employee-form-group';
import { EditEmployeeFormModel } from '../../models/edit-employee-form.model';
import { nameof } from '../../../shared/helpers/name-of.helper';

@Component({
	selector: 'app-edit-employee-dialog',
	templateUrl: './edit-employee-dialog.component.html',
	styleUrls: ['./edit-employee-dialog.component.scss']
})
export class EditEmployeeDialogComponent {
	editEmployeeForm: FormGroup<EditEmployeeFormGroup>;
	departments = DepartmentsArray;
	shiftTypes = ShiftTypesArray;

	constructor(fb: FormBuilder, private store: Store, @Inject(MAT_DIALOG_DATA) employee: EmployeeModel) {
		this.editEmployeeForm = fb.group<EditEmployeeFormGroup>({
			id: new FormControl<number>(employee.id, { nonNullable: true, validators: [Validators.required] }),
			firstName: new FormControl<string>(employee.firstName, {
				nonNullable: true,
				validators: [Validators.required, Validators.max(64)]
			}),
			lastName: new FormControl<string>(employee.lastName, {
				nonNullable: true,
				validators: [Validators.required, Validators.max(64)]
			}),
			baseSalary: new FormControl<number>(employee.baseSalary, {
				nonNullable: true,
				validators: [Validators.required, Validators.min(0)]
			}),
			salaryBonusPercentage: new FormControl<number>(employee.salaryBonusPercentage, {
				nonNullable: true,
				validators: [Validators.required, Validators.min(0)]
			}),
			rcpId: new FormControl<number>(employee.rcpId, { nonNullable: true, validators: [Validators.required, Validators.min(1)] }),
			departmentId: new FormControl<number>(employee.departmentId, { nonNullable: true, validators: [Validators.required] }),
			departmentName: new FormControl<string | null>(employee.departmentName),
			shiftTypeId: new FormControl<number | null>(employee.shiftTypeId, { nonNullable: true, validators: [Validators.required] }),
			shiftTypeName: new FormControl<string | null>(employee.shiftTypeName),
			position: new FormControl<string>(employee.position, { nonNullable: true, validators: [Validators.max(64)] }),
			personalIdentityNumber: new FormControl<string>(employee.personalIdentityNumber, {
				nonNullable: true,
				validators: [Validators.minLength(11), Validators.maxLength(11)]
			})
		});

		this.store.dispatch(new ResetForm({ path: 'employee.editEmployeeForm', value: this.editEmployeeForm.value }));
	}

	onSubmit() {
		console.log(this.editEmployeeForm.valid);
		if (!this.editEmployeeForm.valid) return;

		this.store.dispatch(
			new UpdateFormValue({
				path: 'employee.editEmployeeForm',
				value: this.departments.filter((x) => x.value === this.editEmployeeForm.value.departmentId)[0].key,
				propertyPath: nameof<EditEmployeeFormModel>('departmentName')
			})
		);

		this.store.dispatch(
			new UpdateFormValue({
				path: 'employee.editEmployeeForm',
				value: this.shiftTypes.filter((x) => x.value === this.editEmployeeForm.value.shiftTypeId)[0].key,
				propertyPath: nameof<EditEmployeeFormModel>('shiftTypeName')
			})
		);

		this.store.dispatch(new Edit(this.editEmployeeForm.value.id!, this.editEmployeeForm.value as EditEmployeeFormModel));
	}
}
