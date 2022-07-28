import { Component, OnInit } from '@angular/core';
import { Store } from '@ngxs/store';
import { Employee } from '../../state/employee.action';
import { EmployeeState } from '../../state/employee.state';
import { EmployeeModel } from '../../../shared/models/employee.model';
import { Modal } from '../../../shared/state/modal.action';
import GetAll = Employee.GetAll;
import OpenAddNewEmployeeDialog = Modal.OpenAddNewEmployeeDialog;
import OpenEditEmployeeDialog = Modal.OpenEditEmployeeDialog;

@Component({
	selector: 'app-employee-list',
	templateUrl: './employee-list.component.html',
	styleUrls: ['./employee-list.component.scss']
})
export class EmployeeListComponent implements OnInit {
	employees$ = this.store.select(EmployeeState.getEmployees);

	columnsToDisplay: string[] = ['rcpId', 'firstName', 'lastName', 'department', 'baseSalary', 'percentageBonus', 'actions'];

	constructor(private store: Store) {}

	ngOnInit(): void {
		this.refresh();
	}

	addEmployee() {
		this.store.dispatch(new OpenAddNewEmployeeDialog());
	}

	edit(employee: EmployeeModel) {
		this.store.dispatch(new OpenEditEmployeeDialog(employee));
	}

	refresh() {
		this.store.dispatch(new GetAll());
	}
}
