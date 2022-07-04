import { IWorkingTimeRecordService } from './working-time-record.service.base';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { EmployeeWorkingTimeRecordDetailsModel } from '../models/employee-working-time-record-details.model';
import { Observable } from 'rxjs';

@Injectable()
export class WorkingTimeRecordService extends IWorkingTimeRecordService {
	constructor(httpClient: HttpClient) {
		super(httpClient);
	}

	getAll(year: number, month: number, groupId: number): Observable<EmployeeWorkingTimeRecordDetailsModel[]> {
		const queryParams = new HttpParams().set('year', year).set('month', month).set('groupId', groupId);

		return this.httpClient.get<EmployeeWorkingTimeRecordDetailsModel[]>(`${this.apiEndpoint}/workingTimeRecords`, {
			params: queryParams
		});
	}
}
