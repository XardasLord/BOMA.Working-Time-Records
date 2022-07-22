import { Observable } from 'rxjs';
import { RemoteServiceBase } from 'src/app/shared/services/remote.service.base';
import { EmployeeWorkingTimeRecordDetailsModel } from '../models/employee-working-time-record-details.model';
import { QueryModel } from '../models/query.model';
import { WorkingTimeRecordSummaryDataFormModel } from '../models/working-time-record-summary-data-form.model';

export abstract class IWorkingTimeRecordService extends RemoteServiceBase {
	public abstract getAll(queryModel: QueryModel): Observable<EmployeeWorkingTimeRecordDetailsModel[]>;
	public abstract updateSummaryData(employeeId: number, updateModel: WorkingTimeRecordSummaryDataFormModel): Observable<void>;
}
