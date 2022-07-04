import { Observable } from 'rxjs';
import { RemoteServiceBase } from 'src/app/shared/services/remote.service.base';
import { EmployeeWorkingTimeRecordDetailsModel } from '../models/employee-working-time-record-details.model';

export abstract class IWorkingTimeRecordService extends RemoteServiceBase {
	public abstract getAll(year: number, month: number, groupId: number): Observable<EmployeeWorkingTimeRecordDetailsModel[]>;
}
