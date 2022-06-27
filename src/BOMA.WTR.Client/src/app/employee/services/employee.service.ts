import { IEmployeeService } from './employee.service.base';
import { HttpClient } from '@angular/common/http';
import { EmployeeModel } from '../models/employee.model';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable()
export class EmployeeService extends IEmployeeService {
  constructor(httpClient: HttpClient) {
    super(httpClient);
  }

  getAll(): Observable<EmployeeModel[]> {
    return this.httpClient.get<EmployeeModel[]>(
      `${this.apiEndpoint}/employees`
    );
  }
}
