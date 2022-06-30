import { IEmployeeService } from './employee.service.base';
import { HttpClient } from '@angular/common/http';
import { EmployeeModel } from '../models/employee.model';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { AddNewEmployeeFormModel } from '../models/add-new-employee-form.model';

@Injectable()
export class EmployeeService extends IEmployeeService {
	constructor(httpClient: HttpClient) {
		super(httpClient);
	}

	getAll(): Observable<EmployeeModel[]> {
		return this.httpClient.get<EmployeeModel[]>(`${this.apiEndpoint}/employees`);
	}

	addEmployee(employee: AddNewEmployeeFormModel): Observable<number> {
		return this.httpClient.post<number>(`${this.apiEndpoint}/employees`, employee);
	}
}
