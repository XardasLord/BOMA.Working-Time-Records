import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { QueryModel } from '../models/query.model';
import { RemoteServiceBase } from '../../shared/services/remote.service.base';
import { ReportDataModel } from '../models/report-data.model';

@Injectable()
export class ReportsService extends RemoteServiceBase {
	constructor(httpClient: HttpClient) {
		super(httpClient);
	}

	loadData(queryModel: QueryModel): Observable<ReportDataModel> {
		const queryParams = this.getQueryParams(queryModel);

		return this.httpClient.get<ReportDataModel>(`${this.apiEndpoint}/reports/workingTimeRecords`, {
			params: queryParams
		});
	}

	private getQueryParams(queryModel: QueryModel): HttpParams {
		const startDateStr = this.formatDateLocal(queryModel.startDate);
		const endDateStr = this.formatDateLocal(queryModel.endDate);

		let queryParams = new HttpParams()
			.set('startDate', startDateStr)
			.set('endDate', endDateStr)
			.set('departmentId', queryModel.departmentId)
			.set('shiftId', queryModel.shiftId);

		if (queryModel.searchText) {
			queryParams = queryParams.set('searchText', queryModel.searchText);
		}

		return queryParams;
	}

	private formatDateLocal(date: Date): string {
		const year = date.getFullYear();
		const month = String(date.getMonth() + 1).padStart(2, '0');
		const day = String(date.getDate()).padStart(2, '0');
		return `${year}-${month}-${day}`;
	}
}
