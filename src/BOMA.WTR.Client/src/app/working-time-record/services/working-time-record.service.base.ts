import { Observable } from 'rxjs';
import { RemoteServiceBase } from 'src/app/shared/services/remote.service.base';
import { EmployeeWorkingTimeRecordDetailsModel } from '../models/employee-working-time-record-details.model';
import { QueryModel } from '../models/query.model';

export abstract class IWorkingTimeRecordService extends RemoteServiceBase {
	public abstract getAll(queryModel: QueryModel): Observable<EmployeeWorkingTimeRecordDetailsModel[]>;
}
