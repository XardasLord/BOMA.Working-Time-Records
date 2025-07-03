import { RemoteServiceBase } from '../../shared/services/remote.service.base';
import { Observable } from 'rxjs';
import { EmployeeModel } from '../../shared/models/employee.model';
import { AddNewEmployeeFormModel } from '../models/add-new-employee-form.model';
import { EditEmployeeFormModel } from '../models/edit-employee-form.model';
import { QueryModel } from '../models/query.model';

export abstract class IEmployeeService extends RemoteServiceBase {
	public abstract getAll(queryModel: QueryModel): Observable<EmployeeModel[]>;
	public abstract addEmployee(employee: AddNewEmployeeFormModel): Observable<number>;
	public abstract editEmployee(employeeId: number, employee: EditEmployeeFormModel): Observable<void>;
	public abstract deactivateEmployee(employeeId: number): Observable<void>;
}
