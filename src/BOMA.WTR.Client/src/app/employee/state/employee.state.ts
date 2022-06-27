import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { Employee } from './employee.action';
import { EmployeeModel } from '../models/employee.model';
import { IEmployeeService } from '../services/employee.service.base';
import GetAll = Employee.GetAll;

export interface EmployeeStateModel {
	employees: EmployeeModel[];
}

const EMPLOYEE_STATE_TOKEN = new StateToken<EmployeeStateModel>('employee');
@State<EmployeeStateModel>({
	name: EMPLOYEE_STATE_TOKEN,
	defaults: {
		employees: []
	}
})
@Injectable()
export class EmployeeState {
	constructor(private employeeService: IEmployeeService, private toastService: ToastrService) {}

	@Selector([EMPLOYEE_STATE_TOKEN])
	static getEmployees(state: EmployeeStateModel): EmployeeModel[] {
		return state.employees;
	}

	@Action(GetAll)
	getAll(ctx: StateContext<EmployeeStateModel>, _: GetAll): Observable<EmployeeModel[]> {
		return this.employeeService.getAll().pipe(
			tap((response) => {
				ctx.patchState({
					employees: response
				});
			})
		);
	}
}
