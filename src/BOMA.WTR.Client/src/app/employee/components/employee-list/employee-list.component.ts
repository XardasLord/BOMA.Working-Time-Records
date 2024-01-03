import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngxs/store';
import { nameof } from '../../../shared/helpers/name-of.helper';
import { Employee } from '../../state/employee.action';
import { EmployeeState } from '../../state/employee.state';
import { EmployeeModel } from '../../../shared/models/employee.model';
import { Modal } from '../../../shared/state/modal.action';
import GetAll = Employee.GetAll;
import OpenAddNewEmployeeDialog = Modal.OpenAddNewEmployeeDialog;
import OpenEditEmployeeDialog = Modal.OpenEditEmployeeDialog;
import Deactivate = Employee.Deactivate;
import { MatSort } from '@angular/material/sort';
import { MatLegacyTableDataSource as MatTableDataSource } from '@angular/material/legacy-table';
import { Subscription } from 'rxjs';

@Component({
	selector: 'app-employee-list',
	templateUrl: './employee-list.component.html',
	styleUrls: ['./employee-list.component.scss']
})
export class EmployeeListComponent implements OnInit, AfterViewInit, OnDestroy {
	@ViewChild(MatSort) sort!: MatSort;
	dataSource!: MatTableDataSource<EmployeeModel>;

	employees$ = this.store.select(EmployeeState.getEmployees);

	private subscriptions: Subscription = new Subscription();

	columnsToDisplay: string[] = [
		nameof<EmployeeModel>('rcpId'),
		nameof<EmployeeModel>('firstName'),
		nameof<EmployeeModel>('lastName'),
		nameof<EmployeeModel>('personalIdentityNumber'),
		nameof<EmployeeModel>('departmentName'),
		nameof<EmployeeModel>('shiftTypeName'),
		nameof<EmployeeModel>('position'),
		nameof<EmployeeModel>('baseSalary'),
		nameof<EmployeeModel>('salaryBonusPercentage'),
		'actions'
	];

	constructor(private store: Store) {}

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
}
