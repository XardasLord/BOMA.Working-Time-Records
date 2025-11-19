import { inject, Injectable } from '@angular/core';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { catchError, finalize, Observable, take, tap, throwError } from 'rxjs';
import { IProgressSpinnerService } from '../../shared/services/progress-spinner.base.service';
import { DefaultQueryModel, QueryModel } from '../models/query.model';
import { EmployeeWorkingTimeRecordDetailsModel } from '../../working-time-record/models/employee-working-time-record-details.model';
import { ReportsService } from '../services/reports.service';
import { Reports } from './reports.action';
import LoadData = Reports.LoadData;
import ApplyFilter = Reports.ApplyFilter;
import ChangeGroup = Reports.ChangeGroup;
import ChangeDateRange = Reports.ChangeDateRange;
import ChangeShift = Reports.ChangeShift;
import { ReportDataModel } from '../models/report-data.model';

export interface ReportsStateModel {
	query: QueryModel;
	reportData: ReportDataModel | null;
}

const REPORTS_STATE_TOKEN = new StateToken<ReportsStateModel>('reports');

@State<ReportsStateModel>({
	name: REPORTS_STATE_TOKEN,
	defaults: {
		query: DefaultQueryModel,
		reportData: null
	}
})
@Injectable()
export class ReportsState {
	private reportsService = inject(ReportsService);
	private progressSpinnerService = inject(IProgressSpinnerService);

	@Selector([REPORTS_STATE_TOKEN])
	static getSearchQueryModel(state: ReportsStateModel): QueryModel {
		return state.query;
	}

	@Selector([REPORTS_STATE_TOKEN])
	static getReportData(state: ReportsStateModel): ReportDataModel | null {
		return state.reportData;
	}

	@Action(LoadData)
	loadData(ctx: StateContext<ReportsStateModel>, _: LoadData): Observable<ReportDataModel> {
		this.progressSpinnerService.showProgressSpinner();

		return this.reportsService.loadData(ctx.getState().query).pipe(
			take(1),
			tap((response) => {
				ctx.patchState({
					reportData: response
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
	applyFilter(ctx: StateContext<ReportsStateModel>, action: ApplyFilter): Observable<void> {
		const updatedQuery = { ...ctx.getState().query };
		updatedQuery.searchText = action.searchPhrase;

		ctx.patchState({
			query: updatedQuery
		});

		return ctx.dispatch(new LoadData());
	}

	@Action(ChangeGroup)
	changeGroup(ctx: StateContext<ReportsStateModel>, action: ChangeGroup): Observable<void> {
		const updatedQuery = { ...ctx.getState().query };
		updatedQuery.departmentId = action.groupId;

		ctx.patchState({
			query: updatedQuery
		});

		return ctx.dispatch(new LoadData());
	}

	@Action(ChangeShift)
	changeShift(ctx: StateContext<ReportsStateModel>, action: ChangeShift): Observable<void> {
		const updatedQuery = { ...ctx.getState().query };
		updatedQuery.shiftId = action.shiftId;

		ctx.patchState({
			query: updatedQuery
		});

		return ctx.dispatch(new LoadData());
	}

	@Action(ChangeDateRange)
	changeDateRange(ctx: StateContext<ReportsStateModel>, action: ChangeDateRange): Observable<void> {
		const updatedQuery = { ...ctx.getState().query };
		updatedQuery.startDate = action.startDate;
		updatedQuery.endDate = action.endDate;

		ctx.patchState({
			query: updatedQuery
		});

		return ctx.dispatch(new LoadData());
	}
}
