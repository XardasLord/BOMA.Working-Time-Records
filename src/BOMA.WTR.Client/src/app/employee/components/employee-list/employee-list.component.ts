import { Component, OnInit } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { Employee } from '../../state/employee.action';
import { EmployeeState } from '../../state/employee.state';
import { EmployeeModel } from '../../models/employee.model';
import { Modal } from '../../../shared/state/modal.action';
import GetAll = Employee.GetAll;
import OpenAddNewEmployeeDialog = Modal.OpenAddNewEmployeeDialog;

@Component({
	selector: 'app-employee-list',
	templateUrl: './employee-list.component.html',
	styleUrls: ['./employee-list.component.scss']
})
export class EmployeeListComponent implements OnInit {
	@Select(EmployeeState.getEmployees) employees$!: Observable<EmployeeModel[]>;

	columnsToDisplay: string[] = ['rcpId', 'firstName', 'lastName'];

	constructor(private store: Store) {}

	ngOnInit(): void {
		this.store.dispatch(new GetAll());
	}

	addEmployee() {
		this.store.dispatch(new OpenAddNewEmployeeDialog());
	}
}
