import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { EmployeeWorkingTimeRecordDetailsModel } from '../models/employee-working-time-record-details.model';
import { WorkingTimeRecord } from './working-time-record.action';
import { IWorkingTimeRecordService } from '../services/working-time-record.service.base';
import GetAll = WorkingTimeRecord.GetAll;

export interface WorkingTimeRecordStateModel {
	detailedRecords: EmployeeWorkingTimeRecordDetailsModel[];
}

const WORKING_TIME_RECORD_STATE_TOKEN = new StateToken<WorkingTimeRecordStateModel>('workingTimeRecord');
@State<WorkingTimeRecordStateModel>({
	name: WORKING_TIME_RECORD_STATE_TOKEN,
	defaults: {
		detailedRecords: []
	}
})
@Injectable()
export class WorkingTimeRecordState {
	constructor(private workingTimeRecordService: IWorkingTimeRecordService) {}

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
	getAll(ctx: StateContext<WorkingTimeRecordStateModel>, action: GetAll): Observable<EmployeeWorkingTimeRecordDetailsModel[]> {
		return this.workingTimeRecordService.getAll(action.year, action.month, action.groupId).pipe(
			tap((response) => {
				ctx.patchState({
					detailedRecords: response
				});
			})
		);
	}
}
