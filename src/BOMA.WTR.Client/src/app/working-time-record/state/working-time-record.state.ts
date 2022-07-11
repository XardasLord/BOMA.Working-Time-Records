import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { EmployeeWorkingTimeRecordDetailsModel } from '../models/employee-working-time-record-details.model';
import { WorkingTimeRecord } from './working-time-record.action';
import { IWorkingTimeRecordService } from '../services/working-time-record.service.base';
import GetAll = WorkingTimeRecord.GetAll;
import ApplyFilter = WorkingTimeRecord.ApplyFilter;
import { DefaultQueryModel, QueryModel } from '../models/query.model';
import ChangeGroup = WorkingTimeRecord.ChangeGroup;
import ChangeDate = WorkingTimeRecord.ChangeDate;

export interface WorkingTimeRecordStateModel {
	query: QueryModel;
	detailedRecords: EmployeeWorkingTimeRecordDetailsModel[];
}

const WORKING_TIME_RECORD_STATE_TOKEN = new StateToken<WorkingTimeRecordStateModel>('workingTimeRecord');
@State<WorkingTimeRecordStateModel>({
	name: WORKING_TIME_RECORD_STATE_TOKEN,
	defaults: {
		query: DefaultQueryModel,
		detailedRecords: []
	}
})
@Injectable()
export class WorkingTimeRecordState {
	constructor(private workingTimeRecordService: IWorkingTimeRecordService) {}

	@Selector([WORKING_TIME_RECORD_STATE_TOKEN])
	static getSearchQueryModel(state: WorkingTimeRecordStateModel): QueryModel {
		return state.query;
	}

	@Selector([WORKING_TIME_RECORD_STATE_TOKEN])
	static getDetailedRecords(state: WorkingTimeRecordStateModel): EmployeeWorkingTimeRecordDetailsModel[] {
		return state.detailedRecords;
	}

	@Selector([WORKING_TIME_RECORD_STATE_TOKEN])
	static getDetailedRecordsNormalizedForTable(state: WorkingTimeRecordStateModel): EmployeeWorkingTimeRecordDetailsModel[] {
		const result: any = [];

		// For table binding with rowspan simplicity
		state.detailedRecords.map((x) => {
			const model = new EmployeeWorkingTimeRecordDetailsModel();
			model.employee = x.employee;
			model.salaryInformation = x.salaryInformation;
			model.workingTimeRecordsAggregated = x.workingTimeRecordsAggregated;

			for (let i = 0; i < 6; i++) {
				result.push(model);
			}
		});

		return result;
	}

	@Action(GetAll)
	getAll(ctx: StateContext<WorkingTimeRecordStateModel>, _: GetAll): Observable<EmployeeWorkingTimeRecordDetailsModel[]> {
		const state = ctx.getState();

		return this.workingTimeRecordService.getAll(state.query).pipe(
			tap((response) => {
				ctx.patchState({
					detailedRecords: response
				});
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
		updatedQuery.groupId = action.groupId;

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

		ctx.patchState({
			query: updatedQuery
		});

		return ctx.dispatch(new GetAll());
	}
}
