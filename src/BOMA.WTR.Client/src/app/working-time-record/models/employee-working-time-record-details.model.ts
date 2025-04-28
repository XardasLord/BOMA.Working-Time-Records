import { EmployeeModel } from '../../shared/models/employee.model';
import { WorkingTimeRecordDetailsAggregatedModel } from './working-time-record-details-aggregated.model';
import { SalaryInformationModel } from './salary-information.model';

export interface EmployeeWorkingTimeRecordDetailsModel {
	employee: EmployeeModel;
	salaryInformation: SalaryInformationModel;
	// TODO: BOMA-13: Zwracanie informacji historycznej o aktualnym na ten miesiąc dziale, zmianie i czy jest aktywny pracownik
	workingTimeRecordsAggregated: WorkingTimeRecordDetailsAggregatedModel[];
	isEditable: boolean;
}
