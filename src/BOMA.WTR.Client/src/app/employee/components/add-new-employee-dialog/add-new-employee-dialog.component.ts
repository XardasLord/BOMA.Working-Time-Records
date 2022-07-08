import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngxs/store';
import { Employee } from '../../state/employee.action';
import Add = Employee.Add;

@Component({
	selector: 'app-add-new-employee-dialog',
	templateUrl: './add-new-employee-dialog.component.html',
	styleUrls: ['./add-new-employee-dialog.component.scss']
})
export class AddNewEmployeeDialogComponent implements OnInit {
	addNewEmployeeForm!: FormGroup;

	constructor(fb: FormBuilder, private store: Store) {
		this.addNewEmployeeForm = fb.group({
			firstName: new FormControl<string>('', [Validators.required, Validators.max(64)]),
			lastName: new FormControl<string>('', [Validators.required, Validators.max(64)]),
			baseSalary: new FormControl<number>(0, [Validators.required, Validators.min(0)]),
			salaryBonusPercentage: new FormControl<number>(0, [Validators.required, Validators.min(0)]),
			rcpId: new FormControl<number>(0, [Validators.required, Validators.min(1)])
		});
	}

	ngOnInit(): void {}

	onSubmit() {
		if (!this.addNewEmployeeForm.valid) return;

		this.store.dispatch(new Add(this.addNewEmployeeForm.value));
	}
}
