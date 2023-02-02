import { IEmployeeService } from './employee.service.base';
import { HttpClient } from '@angular/common/http';
import { EmployeeModel } from '../../shared/models/employee.model';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { AddNewEmployeeFormModel } from '../models/add-new-employee-form.model';
import { EditEmployeeFormModel } from '../models/edit-employee-form.model';

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

	editEmployee(employeeId: number, employee: EditEmployeeFormModel): Observable<void> {
		return this.httpClient.put<void>(`${this.apiEndpoint}/employees/${employeeId}`, employee);
	}

	deactivateEmployee(employeeId: number): Observable<void> {
		return this.httpClient.delete<void>(`${this.apiEndpoint}/employees/${employeeId}`);
	}
}
