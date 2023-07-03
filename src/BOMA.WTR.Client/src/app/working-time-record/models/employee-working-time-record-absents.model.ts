import { EmployeeModel } from '../../shared/models/employee.model';
import { WorkingTimeRecordDetailsAggregatedModel } from './working-time-record-details-aggregated.model';
import { SalaryInformationModel } from './salary-information.model';

export interface EmployeeWorkingTimeRecordAbsentsModel {
	employee: EmployeeModel;
	salaryInformation: SalaryInformationModel;
	workingTimeRecordsAggregated: WorkingTimeRecordDetailsAggregatedModel[];
}
