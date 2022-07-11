import { IWorkingTimeRecordService } from './working-time-record.service.base';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { EmployeeWorkingTimeRecordDetailsModel } from '../models/employee-working-time-record-details.model';
import { Observable } from 'rxjs';
import { QueryModel } from '../models/query.model';

@Injectable()
export class WorkingTimeRecordService extends IWorkingTimeRecordService {
	constructor(httpClient: HttpClient) {
		super(httpClient);
	}

	getAll(queryModel: QueryModel): Observable<EmployeeWorkingTimeRecordDetailsModel[]> {
		let queryParams = new HttpParams().set('year', queryModel.year).set('month', queryModel.month).set('groupId', queryModel.groupId);
		console.warn(queryModel.searchText);
		if (queryModel.searchText) {
			queryParams = queryParams.set('searchText', queryModel.searchText);
		}

		return this.httpClient.get<EmployeeWorkingTimeRecordDetailsModel[]>(`${this.apiEndpoint}/workingTimeRecords`, {
			params: queryParams
		});
	}
}
