import { EmployeeModel } from '../../shared/models/employee.model';
import { WorkingTimeRecordDetailsAggregatedModel } from './working-time-record-details-aggregated.model';

export class EmployeeWorkingTimeRecordDetailsModel {
	employee!: EmployeeModel;
	workingTimeRecordsAggregated!: WorkingTimeRecordDetailsAggregatedModel[];
}
