import { IWorkingTimeRecordService } from './working-time-record.service.base';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { EmployeeWorkingTimeRecordDetailsModel } from '../models/employee-working-time-record-details.model';
import { Observable } from 'rxjs';
import { QueryModel } from '../models/query.model';
import {
	WorkingTimeRecordDetailedDataFormModel,
	WorkingTimeRecordSummaryDataFormModel
} from '../models/working-time-record-summary-data-form.model';

@Injectable()
export class WorkingTimeRecordService extends IWorkingTimeRecordService {
	constructor(httpClient: HttpClient) {
		super(httpClient);
	}

	getAll(queryModel: QueryModel): Observable<EmployeeWorkingTimeRecordDetailsModel[]> {
		const queryParams = this.getQueryParams(queryModel);

		return this.httpClient.get<EmployeeWorkingTimeRecordDetailsModel[]>(`${this.apiEndpoint}/workingTimeRecords`, {
			params: queryParams
		});
	}

	updateSummaryData(employeeId: number, updateModel: WorkingTimeRecordSummaryDataFormModel): Observable<void> {
		return this.httpClient.patch<void>(`${this.apiEndpoint}/employees/${employeeId}/workingTimeRecordsSummary`, updateModel);
	}

	updateDetailedData(employeeId: number, updateModel: WorkingTimeRecordDetailedDataFormModel): Observable<void> {
		return this.httpClient.patch<void>(`${this.apiEndpoint}/employees/${employeeId}/workingTimeRecordsDetails`, updateModel);
	}

	sendHoursToGratyfikant(queryModel: QueryModel): Observable<void> {
		const queryParams = this.getQueryParams(queryModel);

		return this.httpClient.post<void>(`${this.apiEndpoint}/workingTimeRecords/gratyfikant`, null, {
			params: queryParams
		});
	}

	private getQueryParams(queryModel: QueryModel): HttpParams {
		let queryParams = new HttpParams()
			.set('year', queryModel.year)
			.set('month', queryModel.month)
			.set('departmentId', queryModel.departmentId)
			.set('shiftId', queryModel.shiftId);

		if (queryModel.searchText) {
			queryParams = queryParams.set('searchText', queryModel.searchText);
		}

		return queryParams;
	}
}
