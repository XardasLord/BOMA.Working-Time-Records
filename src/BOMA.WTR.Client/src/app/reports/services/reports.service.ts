import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { QueryModel } from '../models/query.model';
import { RemoteServiceBase } from '../../shared/services/remote.service.base';
import { EmployeeWorkingTimeRecordDetailsModel } from '../../working-time-record/models/employee-working-time-record-details.model';

@Injectable()
export class ReportsService extends RemoteServiceBase {
	constructor(httpClient: HttpClient) {
		super(httpClient);
	}

	loadData(queryModel: QueryModel): Observable<EmployeeWorkingTimeRecordDetailsModel[]> {
		const queryParams = this.getQueryParams(queryModel);

		return this.httpClient.get<EmployeeWorkingTimeRecordDetailsModel[]>(`${this.apiEndpoint}/reports/workingTimeRecords`, {
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
