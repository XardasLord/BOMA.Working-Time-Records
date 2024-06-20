import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngxs/store';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription } from 'rxjs';
import { nameof } from '../../../shared/helpers/name-of.helper';
import { Employee } from '../../state/employee.action';
import { EmployeeState } from '../../state/employee.state';
import { EmployeeModel } from '../../../shared/models/employee.model';
import { Modal } from '../../../shared/state/modal.action';
import { AuthState } from '../../../shared/auth/state/auth.state';
import { Role } from '../../../shared/auth/models/userDetails';
import GetAll = Employee.GetAll;
import OpenAddNewEmployeeDialog = Modal.OpenAddNewEmployeeDialog;
import OpenEditEmployeeDialog = Modal.OpenEditEmployeeDialog;
import Deactivate = Employee.Deactivate;

@Component({
	selector: 'app-employee-list',
	templateUrl: './employee-list.component.html',
	styleUrls: ['./employee-list.component.scss']
})
export class EmployeeListComponent implements OnInit, AfterViewInit, OnDestroy {
	@ViewChild(MatSort) sort!: MatSort;
	dataSource!: MatTableDataSource<EmployeeModel>;

	employees$ = this.store.select(EmployeeState.getEmployees);
	userRole = this.store.selectSnapshot(AuthState.getUserRole);

	private subscriptions: Subscription = new Subscription();

	columnsToDisplay: string[] = [
		nameof<EmployeeModel>('rcpId'),
		nameof<EmployeeModel>('firstName'),
		nameof<EmployeeModel>('lastName'),
		nameof<EmployeeModel>('departmentName'),
		nameof<EmployeeModel>('shiftTypeName'),
		nameof<EmployeeModel>('position')
	];

	constructor(private store: Store) {
		if ([Role.Admin, Role.UserWithSalaryView, Role.UserWithSalaryEdit].includes(this.userRole!)) {
			this.columnsToDisplay.push(nameof<EmployeeModel>('baseSalary'), nameof<EmployeeModel>('salaryBonusPercentage'));
		}

		if ([Role.Admin, Role.UserWithSalaryEdit].includes(this.userRole!)) {
			this.columnsToDisplay.push('actions');
		}
	}

	ngOnInit(): void {
		this.refresh();
	}

	ngAfterViewInit(): void {
		this.subscriptions.add(
			this.employees$.subscribe((data) => {
				this.dataSource = new MatTableDataSource(data);
				this.dataSource.sort = this.sort;
			})
		);
	}

	ngOnDestroy(): void {
		this.subscriptions.unsubscribe();
	}

	addEmployee() {
		this.store.dispatch(new OpenAddNewEmployeeDialog());
	}

	edit(employee: EmployeeModel) {
		this.store.dispatch(new OpenEditEmployeeDialog(employee));
	}

	deactivate(employee: EmployeeModel) {
		this.store.dispatch(new Deactivate(employee.id));
	}

	refresh() {
		this.store.dispatch(new GetAll());
	}

	protected readonly Role = Role;
}
