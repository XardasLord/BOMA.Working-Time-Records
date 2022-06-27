import { RemoteServiceBase } from '../../shared/services/remote.service.base';
import { Observable } from 'rxjs';
import { EmployeeModel } from '../models/employee.model';

export abstract class IEmployeeService extends RemoteServiceBase {
  public abstract getAll(): Observable<EmployeeModel[]>;
}
