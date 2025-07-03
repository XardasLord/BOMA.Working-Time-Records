import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';

import { IEmployeeService } from './employee.service.base';
import { EmployeeModel } from '../../shared/models/employee.model';
import { AddNewEmployeeFormModel } from '../models/add-new-employee-form.model';
import { EditEmployeeFormModel } from '../models/edit-employee-form.model';
import { QueryModel } from '../models/query.model';

@Injectable()
export class EmployeeService extends IEmployeeService {
	constructor(httpClient: HttpClient) {
		super(httpClient);
	}

	getAll(queryModel: QueryModel): Observable<EmployeeModel[]> {
		const queryParams = this.getQueryParams(queryModel);

		return this.httpClient.get<EmployeeModel[]>(`${this.apiEndpoint}/employees`, {
			params: queryParams
		});
	}

	addEmployee(employee: AddNewEmployeeFormModel): Observable<number> {
		return this.httpClient.post<number>(`${this.apiEndpoint}/employees`, employee);
	}

	editEmployee(employeeId: number, employee: EditEmployeeFormModel): Observable<void> {
		return this.httpClient.put<void>(`${this.apiEndpoint}/employees/${employeeId}`, employee);
	}

	deactivateEmployee(employeeId: number): Observable<void> {
		return this.httpClient.delete<void>(`${this.apiEndpoint}/employees/${employeeId}`);
	}

	private getQueryParams(queryModel: QueryModel): HttpParams {
		let queryParams = new HttpParams().set('departmentId', queryModel.departmentId).set('shiftId', queryModel.shiftId);

		if (queryModel.searchText) {
			queryParams = queryParams.set('searchText', queryModel.searchText);
		}

		return queryParams;
	}
}
