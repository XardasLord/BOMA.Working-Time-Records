import { HttpErrorResponse } from '@angular/common/http';
import { inject, Injectable, NgZone } from '@angular/core';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { catchError, finalize, Observable, of, switchMap, take, tap, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { IProgressSpinnerService } from '../../shared/services/progress-spinner.base.service';
import { EmployeeWorkingTimeRecordDetailsModel } from '../models/employee-working-time-record-details.model';
import { WorkingTimeRecord } from './working-time-record.action';
import { IWorkingTimeRecordService } from '../services/working-time-record.service.base';
import {
	DefaultColumnsToDisplayForAbsentReportTable,
	DefaultColumnsToDisplayForDetailedTable,
	DefaultColumnsToDisplayForReportHoursTable,
	DefaultInitialDayColumnsToDisplayForDetailedTable,
	DefaultQueryModel,
	NumberOfDaysInMonth,
	QueryModel
} from '../models/query.model';
import GetAll = WorkingTimeRecord.GetAll;
import ApplyFilter = WorkingTimeRecord.ApplyFilter;
import ChangeGroup = WorkingTimeRecord.ChangeGroup;
import ChangeDate = WorkingTimeRecord.ChangeDate;
import { DefaultFormStateValue, FormStateModel } from '../../shared/models/form-states.model';
import { WorkingTimeRecordSummaryDataFormModel } from '../models/working-time-record-summary-data-form.model';
import UpdateSummaryData = WorkingTimeRecord.UpdateSummaryData;
import UpdateDetailedData = WorkingTimeRecord.UpdateDetailedData;
import ChangeShift = WorkingTimeRecord.ChangeShift;
import { EmployeeWorkingTimeRecordAbsentsModel } from '../models/employee-working-time-record-absents.model';
import { WorkingTimeRecordAbsentAggregatedModel } from '../models/working-time-record-details-aggregated.model';
import SendHoursToGratyfikant = WorkingTimeRecord.SendHoursToGratyfikant;
import UpdateReportHoursData = WorkingTimeRecord.UpdateReportHoursData;
import {
	ConfirmationDialogComponent,
	ConfirmationDialogModel
} from '../../shared/components/confirmation-dialog/confirmation-dialog.component';
import { patch, removeItem } from '@ngxs/store/operators';
import { EmployeeModel } from '../../shared/models/employee.model';
import ClosePreviousMonth = WorkingTimeRecord.ClosePreviousMonth;
import { MatDialog } from '@angular/material/dialog';
import {
	InformationDialogComponent,
	InformationDialogModel
} from '../../shared/components/information-dialog/information-dialog.component';
import { KeyValuePair } from '../models/key-value-pair.model';
import { DepartmentsEnum } from '../../shared/models/departments.enum';
import { DepartmentsArray } from '../../shared/models/departments-array';
import { ShiftTypesEnum } from '../../shared/models/shift-types.enum';
import { ShiftTypesArray } from '../../shared/models/shift-types-array';

export interface WorkingTimeRecordStateModel {
	query: QueryModel;
	detailedRecords: EmployeeWorkingTimeRecordDetailsModel[];
	numberOfDays: number;
	columnsToDisplay: string[];
	columnsToDisplayForReportHours: string[];
	columnsToDisplayForAbsentReport: string[];
	summaryForm: FormStateModel<WorkingTimeRecordSummaryDataFormModel>;
}

const WORKING_TIME_RECORD_STATE_TOKEN = new StateToken<WorkingTimeRecordStateModel>('workingTimeRecord');
@State<WorkingTimeRecordStateModel>({
	name: WORKING_TIME_RECORD_STATE_TOKEN,
	defaults: {
		query: DefaultQueryModel,
		detailedRecords: [],
		numberOfDays: NumberOfDaysInMonth(DefaultQueryModel.year, DefaultQueryModel.month),
		columnsToDisplay: [
			...DefaultColumnsToDisplayForDetailedTable,
			...DefaultInitialDayColumnsToDisplayForDetailedTable(DefaultQueryModel.year, DefaultQueryModel.month)
		],
		columnsToDisplayForReportHours: [
			...DefaultColumnsToDisplayForReportHoursTable,
			...DefaultInitialDayColumnsToDisplayForDetailedTable(DefaultQueryModel.year, DefaultQueryModel.month)
		],
		columnsToDisplayForAbsentReport: [
			...DefaultColumnsToDisplayForAbsentReportTable,
			...DefaultInitialDayColumnsToDisplayForDetailedTable(DefaultQueryModel.year, DefaultQueryModel.month)
		],
		summaryForm: DefaultFormStateValue
	}
})
@Injectable()
export class WorkingTimeRecordState {
	private zone = inject(NgZone);
	private workingTimeRecordService = inject(IWorkingTimeRecordService);
	private progressSpinnerService = inject(IProgressSpinnerService);
	private toastService = inject(ToastrService);
	private dialogRef = inject(MatDialog);

	@Selector([WORKING_TIME_RECORD_STATE_TOKEN])
	static getSearchQueryModel(state: WorkingTimeRecordStateModel): QueryModel {
		return state.query;
	}

	@Selector([WORKING_TIME_RECORD_STATE_TOKEN])
	static getDetailedRecords(state: WorkingTimeRecordStateModel): EmployeeWorkingTimeRecordDetailsModel[] {
		return state.detailedRecords;
	}

	@Selector([WORKING_TIME_RECORD_STATE_TOKEN])
	static getDaysInMonth(state: WorkingTimeRecordStateModel): string[] {
		const daysArray: string[] = [];
		for (let i = 1; i <= state.numberOfDays; i++) {
			daysArray.push(i.toString());
		}

		return daysArray;
	}

	@Selector([WORKING_TIME_RECORD_STATE_TOKEN])
	static getColumnsToDisplay(state: WorkingTimeRecordStateModel): string[] {
		return state.columnsToDisplay;
	}

	@Selector([WORKING_TIME_RECORD_STATE_TOKEN])
	static getColumnsToDisplayForReportHours(state: WorkingTimeRecordStateModel): string[] {
		return state.columnsToDisplayForReportHours;
	}

	@Selector([WORKING_TIME_RECORD_STATE_TOKEN])
	static getColumnsToDisplayForAbsentReportTable(state: WorkingTimeRecordStateModel): string[] {
		return state.columnsToDisplayForAbsentReport;
	}

	@Selector([WORKING_TIME_RECORD_STATE_TOKEN])
	static getDetailedRecordsNormalizedForTable(state: WorkingTimeRecordStateModel): EmployeeWorkingTimeRecordDetailsModel[] {
		const result: EmployeeWorkingTimeRecordDetailsModel[] = [];

		// For table binding with rowspan simplicity
		state.detailedRecords.map((x) => {
			const model: EmployeeWorkingTimeRecordDetailsModel = {
				employee: x.employee,
				salaryInformation: x.salaryInformation,
				workingTimeRecordsAggregated: x.workingTimeRecordsAggregated,
				isEditable: x.isEditable
			};

			for (let i = 0; i < 6; i++) {
				result.push(model);
			}
		});

		return result;
	}

	@Selector([WORKING_TIME_RECORD_STATE_TOKEN])
	static getReportHourRecordsNormalizedForTable(state: WorkingTimeRecordStateModel): EmployeeWorkingTimeRecordDetailsModel[] {
		const result: EmployeeWorkingTimeRecordDetailsModel[] = [];

		// For table binding with rowspan simplicity
		state.detailedRecords.map((x) => {
			const model: EmployeeWorkingTimeRecordDetailsModel = {
				employee: x.employee,
				salaryInformation: x.salaryInformation,
				workingTimeRecordsAggregated: x.workingTimeRecordsAggregated,
				isEditable: !x.isEditable // `x.isEditable` describe if it is ready for historical changes. We should rename it to be more descriptive
			};

			for (let i = 0; i < 3; i++) {
				result.push(model);
			}
		});

		return result;
	}

	@Selector([WORKING_TIME_RECORD_STATE_TOKEN])
	static getReportAbsentEmployeesRecordsNormalizedForTable(state: WorkingTimeRecordStateModel): EmployeeWorkingTimeRecordAbsentsModel[] {
		const result: EmployeeWorkingTimeRecordAbsentsModel[] = [];
		const now = new Date();

		// For table binding with rowspan simplicity
		state.detailedRecords
			.filter((x) =>
				x.workingTimeRecordsAggregated.some((h) => h.workedHoursRounded === 0 && h.isWeekendDay === false && new Date(h.date) < now)
			)
			.map((x) => {
				const absentRecords: WorkingTimeRecordAbsentAggregatedModel[] = x.workingTimeRecordsAggregated.map((record) => {
					const { date, isWeekendDay, missingRecordEventType } = record;
					const isAbsent = record.workedHoursRounded === 0 && record.isWeekendDay === false && new Date(record.date) < now;

					return {
						date,
						isWeekendDay,
						missingRecordEventType,
						isAbsent
					};
				});
				const model: EmployeeWorkingTimeRecordAbsentsModel = {
					employee: x.employee,
					salaryInformation: x.salaryInformation,
					workingTimeRecordsAggregated: absentRecords
				};

				result.push(model);
			});

		return result;
	}

	@Action(GetAll)
	getAll(ctx: StateContext<WorkingTimeRecordStateModel>, _: GetAll): Observable<EmployeeWorkingTimeRecordDetailsModel[]> {
		this.progressSpinnerService.showProgressSpinner();

		return this.workingTimeRecordService.getAll(ctx.getState().query).pipe(
			take(1),
			tap((response) => {
				ctx.patchState({
					detailedRecords: response
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
	applyFilter(ctx: StateContext<WorkingTimeRecordStateModel>, action: ApplyFilter): Observable<void> {
		const updatedQuery = { ...ctx.getState().query };
		updatedQuery.searchText = action.searchPhrase;

		ctx.patchState({
			query: updatedQuery
		});

		return ctx.dispatch(new GetAll());
	}

	@Action(ChangeGroup)
	changeGroup(ctx: StateContext<WorkingTimeRecordStateModel>, action: ChangeGroup): Observable<void> {
		const updatedQuery = { ...ctx.getState().query };
		updatedQuery.departmentId = action.groupId;

		ctx.patchState({
			query: updatedQuery
		});

		return ctx.dispatch(new GetAll());
	}

	@Action(ChangeShift)
	changeShift(ctx: StateContext<WorkingTimeRecordStateModel>, action: ChangeShift): Observable<void> {
		const updatedQuery = { ...ctx.getState().query };
		updatedQuery.shiftId = action.shiftId;

		ctx.patchState({
			query: updatedQuery
		});

		return ctx.dispatch(new GetAll());
	}

	@Action(ChangeDate)
	changeDate(ctx: StateContext<WorkingTimeRecordStateModel>, action: ChangeDate): Observable<void> {
		const updatedQuery = { ...ctx.getState().query };
		updatedQuery.year = action.year;
		updatedQuery.month = action.month;

		const numberOfDays = new Date(updatedQuery.year, updatedQuery.month, 0).getDate();

		ctx.patchState({
			query: updatedQuery,
			numberOfDays: numberOfDays,
			columnsToDisplay: DefaultColumnsToDisplayForDetailedTable,
			columnsToDisplayForReportHours: DefaultColumnsToDisplayForReportHoursTable
		});

		for (let i = 1; i <= numberOfDays; i++) {
			ctx.patchState({
				columnsToDisplay: [...ctx.getState().columnsToDisplay, i.toString()],
				columnsToDisplayForReportHours: [...ctx.getState().columnsToDisplayForReportHours, i.toString()]
			});
		}

		return ctx.dispatch(new GetAll());
	}

	@Action(UpdateSummaryData)
	updateSummaryData(ctx: StateContext<WorkingTimeRecordStateModel>, action: UpdateSummaryData): Observable<void> {
		this.progressSpinnerService.showProgressSpinner();

		return this.workingTimeRecordService.updateSummaryData(action.employeeId, action.updateModel).pipe(
			tap(() => {
				// Update state is not needed, because we need to recalculate the volumes that are done by API

				ctx.dispatch(new GetAll());
				this.toastService.success('Dane zostały zapisane');
			}),
			catchError((e: HttpErrorResponse) => {
				this.toastService.error(`Wystąpił błąd przy aktualizacji danych - ${e.message}`);
				return throwError(() => e);
			}),
			finalize(() => {
				this.progressSpinnerService.hideProgressSpinner();
			})
		);
	}

	@Action(UpdateDetailedData)
	updateDetailedData(ctx: StateContext<WorkingTimeRecordStateModel>, action: UpdateDetailedData): Observable<void> {
		this.progressSpinnerService.showProgressSpinner();

		return this.workingTimeRecordService.updateDetailedData(action.employeeId, action.updateModel).pipe(
			tap(() => {
				// This is not needed to update state, because we need to recalculate the sums that API does only

				ctx.dispatch(new GetAll());
				this.toastService.success('Dane zostały zapisane');
			}),
			catchError((e: HttpErrorResponse) => {
				this.toastService.error(`Wystąpił błąd przy aktualizacji danych - ${e.message}`);
				return throwError(() => e);
			}),
			finalize(() => {
				this.progressSpinnerService.hideProgressSpinner();
			})
		);
	}

	@Action(UpdateReportHoursData)
	updateReportHoursData(ctx: StateContext<WorkingTimeRecordStateModel>, action: UpdateReportHoursData): Observable<void> {
		this.progressSpinnerService.showProgressSpinner();

		return this.workingTimeRecordService.updateReportHoursData(action.employeeId, action.updateModel).pipe(
			tap(() => {
				// This is not needed to update state, because we need to recalculate hours that API does only

				ctx.dispatch(new GetAll());
				this.toastService.success('Dane zostały zapisane');
			}),
			catchError((e: HttpErrorResponse) => {
				this.toastService.error(`Wystąpił błąd przy aktualizacji danych - ${e.message}`);
				return throwError(() => e);
			}),
			finalize(() => {
				this.progressSpinnerService.hideProgressSpinner();
			})
		);
	}

	@Action(SendHoursToGratyfikant)
	sendHoursToGratyfikant(ctx: StateContext<WorkingTimeRecordStateModel>) {
		const message = `Czy na pewno chcesz wysłać aktualnie widoczne godziny za podany okres do Gratyfikanta?
<br>Rok: <b>${ctx.getState().query.year}</b>
<br>Miesiąc: <b>${ctx.getState().query.month}</b>
<br>Dział: <b>${DepartmentsArray.filter((x) => x.value === ctx.getState().query.departmentId)[0].key}</b>
<br>Zmiana: <b>${ShiftTypesArray.filter((x) => x.value === ctx.getState().query.shiftId)[0].key}</b>
<br><br>Po synchronizacji otrzymasz szczegółową informację o błędach, które wystąpiły podczas wysyłki danych do Gratyfikanta.`;

		const dialogData = new ConfirmationDialogModel('Potwierdzenie wysłania godzin', message);

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

					this.progressSpinnerService.showProgressSpinner();

					return this.workingTimeRecordService.sendHoursToGratyfikant(ctx.getState().query).pipe(
						tap((response) => {
							this.toastService.success('Dane zostały wysłane do Gratyfikanta', 'Synchronizacja danych', {
								onActivateTick: true
							});

							const resultMessage = `<u>Poniżej znajduje się lista błędów związanych z wysyłką danych do Gratyfikanta:</u>${response.map((e) => `<li>${e}</li>`).join('')}`;
							const resultDialogData = new InformationDialogModel('Informacja z synchronizacji godzin ', resultMessage);

							this.zone.run(() =>
								this.dialogRef.open(InformationDialogComponent, {
									maxWidth: '600px',
									data: resultDialogData
								})
							);
						}),
						catchError((e: HttpErrorResponse) => {
							this.toastService.error(`Wystąpił błąd przy aktualizacji danych - ${e.message}`, 'Synchronizacja danych', {
								onActivateTick: true
							});

							return throwError(() => e);
						}),
						finalize(() => {
							this.progressSpinnerService.hideProgressSpinner();
						})
					);
				})
			);
	}

	@Action(ClosePreviousMonth)
	closePreviousMonth(ctx: StateContext<WorkingTimeRecordStateModel>, action: ClosePreviousMonth) {
		const message = `Czy na pewno chcesz zamknąć poprzedni miesiąc?`;

		const dialogData = new ConfirmationDialogModel('Potwierdzenie zamknięcia miesiąca', message);

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

					this.progressSpinnerService.showProgressSpinner();

					return this.workingTimeRecordService.closePreviousMonth().pipe(
						tap((_) => {
							this.toastService.success(
								`Zamykanie poprzedniego miesiąca zostało rozpoczęte. Może to potrwać kilka minut.`,
								'Sukces'
							);
						}),
						catchError((e: HttpErrorResponse) => {
							this.toastService.error(`Wystąpił błąd przy zamykaniu miesiąca - ${e.message}`);
							return throwError(() => e);
						}),
						finalize(() => {
							this.progressSpinnerService.hideProgressSpinner();
						})
					);
				})
			);
	}
}
