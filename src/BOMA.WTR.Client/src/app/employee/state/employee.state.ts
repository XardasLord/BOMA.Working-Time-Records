import { Injectable, NgZone } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { HttpErrorResponse } from '@angular/common/http';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { append, patch, removeItem, updateItem } from '@ngxs/store/operators';
import { catchError, finalize, Observable, of, switchMap, tap, throwError } from 'rxjs';
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
import Deactivate = Employee.Deactivate;
import {
	ConfirmationDialogComponent,
	ConfirmationDialogModel
} from '../../shared/components/confirmation-dialog/confirmation-dialog.component';
import { DefaultQueryModel, QueryModel } from '../models/query.model';
import ApplyFilter = Employee.ApplyFilter;
import ChangeGroup = Employee.ChangeGroup;
import ChangeShift = Employee.ChangeShift;

export interface EmployeeStateModel {
	employees: EmployeeModel[];
	query: QueryModel;
	addNewEmployeeForm: FormStateModel<AddNewEmployeeFormModel>;
	editEmployeeForm: FormStateModel<EditEmployeeFormModel>;
}

const EMPLOYEE_STATE_TOKEN = new StateToken<EmployeeStateModel>('employee');
@State<EmployeeStateModel>({
	name: EMPLOYEE_STATE_TOKEN,
	defaults: {
		employees: [],
		query: DefaultQueryModel,
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
		private dialogRef: MatDialog,
		private zone: NgZone
	) {}

	@Selector([EMPLOYEE_STATE_TOKEN])
	static getEmployees(state: EmployeeStateModel): EmployeeModel[] {
		return state.employees;
	}

	@Selector([EMPLOYEE_STATE_TOKEN])
	static getQueryModel(state: EmployeeStateModel): QueryModel {
		return state.query;
	}

	@Action(GetAll)
	getAll(ctx: StateContext<EmployeeStateModel>, _: GetAll): Observable<EmployeeModel[]> {
		this.progressSpinnerService.showProgressSpinner();

		return this.employeeService.getAll(ctx.getState().query).pipe(
			tap((response) => {
				ctx.patchState({
					employees: response
				});
			}),
			catchError((e) => {
				return throwError(e);
			}),
			finalize(() => {
				this.progressSpinnerService.hideProgressSpinner();
			})
		);
	}

	@Action(ApplyFilter)
	applyFilter(ctx: StateContext<EmployeeStateModel>, action: ApplyFilter): Observable<void> {
		const updatedQuery = { ...ctx.getState().query };
		updatedQuery.searchText = action.searchPhrase;

		ctx.patchState({
			query: updatedQuery
		});

		return ctx.dispatch(new GetAll());
	}

	@Action(ChangeGroup)
	changeGroup(ctx: StateContext<EmployeeStateModel>, action: ChangeGroup): Observable<void> {
		const updatedQuery = { ...ctx.getState().query };
		updatedQuery.departmentId = action.groupId;

		ctx.patchState({
			query: updatedQuery
		});

		return ctx.dispatch(new GetAll());
	}

	@Action(ChangeShift)
	changeShift(ctx: StateContext<EmployeeStateModel>, action: ChangeShift): Observable<void> {
		const updatedQuery = { ...ctx.getState().query };
		updatedQuery.shiftId = action.shiftId;

		ctx.patchState({
			query: updatedQuery
		});

		return ctx.dispatch(new GetAll());
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
								position: action.employee.position,
								isActive: true
							}
						])
					})
				);

				this.toastService.success(
					`Nowy pracownik został dodany - ${action.employee.firstName} ${action.employee.lastName}`,
					'Sukces'
				);

				this.dialogRef.closeAll();
			}),
			catchError((e: HttpErrorResponse) => {
				this.toastService.error(`Wystąpił błąd przy dodawaniu pracownika - ${e.message}`);
				return throwError(() => e);
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
								position: action.employee.position,
								isActive: true
							})
						)
					})
				);

				this.toastService.success(`Pracownik został edytowany`, 'Sukces');

				this.dialogRef.closeAll();
			}),
			catchError((e: HttpErrorResponse) => {
				this.toastService.error(`Wystąpił błąd przy edycji pracownika - ${e.message}`);
				return throwError(() => e);
			})
		);
	}

	@Action(Deactivate)
	deactivate(ctx: StateContext<EmployeeStateModel>, action: Deactivate) {
		const message = `Czy na pewno chcesz dezaktywować pracownika?`;

		const dialogData = new ConfirmationDialogModel('Potwierdzenie dezaktywacji pracownika', message);

		return this.zone
			.run(() =>
				this.dialogRef.open(ConfirmationDialogComponent, {
					maxWidth: '400px',
					data: dialogData
				})
			)
			.afterClosed()
			.pipe(
				switchMap((dialogResultAction) => {
					if (dialogResultAction === undefined || dialogResultAction === false) {
						return of({});
					}

					return this.employeeService.deactivateEmployee(action.employeeId).pipe(
						tap((_) => {
							ctx.setState(
								patch({
									employees: removeItem<EmployeeModel>((x) => x?.id === action.employeeId)
								})
							);

							this.toastService.success(`Pracownik został deaktywowany`, 'Sukces');
						})
					);
				})
			);
	}
}
