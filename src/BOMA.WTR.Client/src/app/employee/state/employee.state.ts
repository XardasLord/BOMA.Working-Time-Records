import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { append, patch, updateItem } from '@ngxs/store/operators';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { IProgressSpinnerService } from '../../shared/services/progress-spinner.base.service';
import { EmployeeModel } from '../../shared/models/employee.model';
import { IEmployeeService } from '../services/employee.service.base';
import { DefaultFormStateValue, FormStateModel } from '../../shared/models/form-states.model';
import { AddNewEmployeeFormModel } from '../models/add-new-employee-form.model';
import { EditEmployeeFormModel } from '../models/edit-employee-form.model';
import { Employee } from './employee.action';
import GetAll = Employee.GetAll;
import Add = Employee.Add;
import Edit = Employee.Edit;

export interface EmployeeStateModel {
	employees: EmployeeModel[];
	addNewEmployeeForm: FormStateModel<AddNewEmployeeFormModel>;
	editEmployeeForm: FormStateModel<EditEmployeeFormModel>;
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
	constructor(
		private employeeService: IEmployeeService,
		private toastService: ToastrService,
		private progressSpinnerService: IProgressSpinnerService,
		private dialogRef: MatDialog
	) {}

	@Selector([EMPLOYEE_STATE_TOKEN])
	static getEmployees(state: EmployeeStateModel): EmployeeModel[] {
		return state.employees;
	}

	@Action(GetAll)
	getAll(ctx: StateContext<EmployeeStateModel>, _: GetAll): Observable<EmployeeModel[]> {
		this.progressSpinnerService.showProgressSpinner();

		return this.employeeService.getAll().pipe(
			tap((response) => {
				this.progressSpinnerService.hideProgressSpinner();

				ctx.patchState({
					employees: response
				});
			}),
			catchError((e) => {
				this.progressSpinnerService.hideProgressSpinner();
				return throwError(e);
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
								departmentName: action.employee.departmentName,
								shiftTypeId: action.employee.shiftTypeId,
								shiftTypeName: action.employee.shiftTypeName,
								position: action.employee.position
							}
						])
					})
				);

				this.toastService.success(
					`Nowy pracownik został dodany - ${action.employee.firstName} ${action.employee.lastName}`,
					'Sukces'
				);
				this.dialogRef.closeAll();
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
								departmentName: action.employee.departmentName,
								shiftTypeId: action.employee.shiftTypeId,
								shiftTypeName: action.employee.shiftTypeName,
								position: action.employee.position
							})
						)
					})
				);

				this.toastService.success(`Pracownik został edytowany`, 'Sukces');
				this.dialogRef.closeAll();
			})
		);
	}
}
