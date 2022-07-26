import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { Injectable } from '@angular/core';
import { patch, updateItem } from '@ngxs/store/operators';
import { catchError, Observable, take, tap, throwError } from 'rxjs';
import { IProgressSpinnerService } from '../../shared/services/progress-spinner.base.service';
import { EmployeeWorkingTimeRecordDetailsModel } from '../models/employee-working-time-record-details.model';
import { WorkingTimeRecord } from './working-time-record.action';
import { IWorkingTimeRecordService } from '../services/working-time-record.service.base';
import {
	DefaultColumnsToDisplayForDetailedTable,
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
import {
	WorkingTimeRecordDetailedDataFormModel,
	WorkingTimeRecordSummaryDataFormModel
} from '../models/working-time-record-summary-data-form.model';
import UpdateSummaryData = WorkingTimeRecord.UpdateSummaryData;

export interface WorkingTimeRecordStateModel {
	query: QueryModel;
	detailedRecords: EmployeeWorkingTimeRecordDetailsModel[];
	numberOfDays: number;
	columnsToDisplay: string[];
	summaryForm: FormStateModel<WorkingTimeRecordSummaryDataFormModel>;
	detailedHoursForm: FormStateModel<WorkingTimeRecordDetailedDataFormModel>;
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
		summaryForm: DefaultFormStateValue,
		detailedHoursForm: DefaultFormStateValue
	}
})
@Injectable()
export class WorkingTimeRecordState {
	constructor(private workingTimeRecordService: IWorkingTimeRecordService, private progressSpinnerService: IProgressSpinnerService) {}

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
	static getDetailedRecordsNormalizedForTable(state: WorkingTimeRecordStateModel): EmployeeWorkingTimeRecordDetailsModel[] {
		const result: any = [];

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

	@Action(GetAll)
	getAll(ctx: StateContext<WorkingTimeRecordStateModel>, _: GetAll): Observable<EmployeeWorkingTimeRecordDetailsModel[]> {
		const state = ctx.getState();

		this.progressSpinnerService.showProgressSpinner();

		return this.workingTimeRecordService.getAll(state.query).pipe(
			take(1),
			tap((response) => {
				ctx.patchState({
					detailedRecords: response
				});

				this.progressSpinnerService.hideProgressSpinner();
			}),
			catchError((e) => {
				this.progressSpinnerService.hideProgressSpinner();
				return throwError(e);
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

	@Action(ChangeDate)
	changeDate(ctx: StateContext<WorkingTimeRecordStateModel>, action: ChangeDate): Observable<void> {
		const updatedQuery = { ...ctx.getState().query };
		updatedQuery.year = action.year;
		updatedQuery.month = action.month;

		const numberOfDays = new Date(updatedQuery.year, updatedQuery.month, 0).getDate();

		ctx.patchState({
			query: updatedQuery,
			numberOfDays: numberOfDays,
			columnsToDisplay: DefaultColumnsToDisplayForDetailedTable
		});

		for (let i = 1; i <= numberOfDays; i++) {
			ctx.patchState({
				columnsToDisplay: [...ctx.getState().columnsToDisplay, i.toString()]
			});
		}

		return ctx.dispatch(new GetAll());
	}

	@Action(UpdateSummaryData)
	updateSummaryData(ctx: StateContext<WorkingTimeRecordStateModel>, action: UpdateSummaryData): Observable<void> {
		this.progressSpinnerService.showProgressSpinner();

		return this.workingTimeRecordService.updateSummaryData(action.employeeId, action.updateModel).pipe(
			tap(() => {
				this.progressSpinnerService.hideProgressSpinner();

				ctx.setState(
					patch({
						detailedRecords: updateItem<EmployeeWorkingTimeRecordDetailsModel>(
							(record) => record?.employee.id === action.employeeId,
							patch({
								salaryInformation: patch({
									holidaySalary: action.updateModel.holidaySalary
									// finalSumSalary // TODO: update
								})
							})
						)
					})
				);

				ctx.dispatch(new GetAll());
			}),
			catchError((e) => {
				this.progressSpinnerService.hideProgressSpinner();
				return throwError(e);
			})
		);
	}
}
