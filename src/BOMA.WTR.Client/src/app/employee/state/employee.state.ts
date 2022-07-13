import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { Employee } from './employee.action';
import { EmployeeModel } from '../../shared/models/employee.model';
import { IEmployeeService } from '../services/employee.service.base';
import { DefaultFormStateValue, FormStateModel } from '../../shared/models/form-states.model';
import { AddNewEmployeeFormModel } from '../models/add-new-employee-form.model';
import GetAll = Employee.GetAll;
import Add = Employee.Add;
import { append, patch, updateItem } from '@ngxs/store/operators';
import Edit = Employee.Edit;

export interface EmployeeStateModel {
	employees: EmployeeModel[];
	addNewEmployeeForm: FormStateModel<AddNewEmployeeFormModel>;
	editEmployeeForm: FormStateModel<AddNewEmployeeFormModel>;
}

const EMPLOYEE_STATE_TOKEN = new StateToken<EmployeeStateModel>('employee');
@State<EmployeeStateModel>({
	name: EMPLOYEE_STATE_TOKEN,
	defaults: {
		employees: [],
		addNewEmployeeForm: DefaultFormStateValue,
		editEmployeeForm: DefaultFormStateValue
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

	@Action(Add)
	add(ctx: StateContext<EmployeeStateModel>, action: Add): Observable<number> {
		return this.employeeService.addEmployee(action.employee).pipe(
			tap((employeeId) => {
				ctx.setState(
					patch({
						employees: append<EmployeeModel>([
							{
								id: employeeId,
								firstName: action.employee.firstName,
								lastName: action.employee.lastName,
								baseSalary: action.employee.baseSalary,
								salaryBonusPercentage: action.employee.salaryBonusPercentage,
								rcpId: action.employee.rcpId,
								departmentId: action.employee.departmentId,
								departmentName: action.employee.departmentName
							}
						])
					})
				);

				this.toastService.success(
					`Nowy pracownik został dodany - ${action.employee.firstName} ${action.employee.lastName}`,
					'Sukces'
				);
			})
		);
	}

	@Action(Edit)
	edit(ctx: StateContext<EmployeeStateModel>, action: Edit): Observable<void> {
		return this.employeeService.editEmployee(action.employeeId, action.employee).pipe(
			tap((_) => {
				ctx.setState(
					patch({
						employees: updateItem<EmployeeModel>(
							(x) => x?.id === action.employeeId,
							patch({
								id: action.employeeId,
								firstName: action.employee.firstName,
								lastName: action.employee.lastName,
								baseSalary: action.employee.baseSalary,
								salaryBonusPercentage: action.employee.salaryBonusPercentage,
								rcpId: action.employee.rcpId,
								departmentId: action.employee.departmentId,
								departmentName: action.employee.departmentName
							})
						)
					})
				);

				this.toastService.success(`Pracownik został edytowany`, 'Sukces');
			})
		);
	}
}
