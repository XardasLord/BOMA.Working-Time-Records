import { RemoteServiceBase } from '../../shared/services/remote.service.base';
import { Observable } from 'rxjs';
import { EmployeeModel } from '../models/employee.model';
import { AddNewEmployeeFormModel } from '../models/add-new-employee-form.model';

export abstract class IEmployeeService extends RemoteServiceBase {
	public abstract getAll(): Observable<EmployeeModel[]>;
	public abstract addEmployee(employee: AddNewEmployeeFormModel): Observable<number>;
}
