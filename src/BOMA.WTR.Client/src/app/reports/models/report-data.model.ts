export interface ReportDataModel {
	workingHoursReport: WorkingHoursReportViewModel;
	salaryReport: SalaryReportViewModel;
}

export interface WorkingHoursReportViewModel {
	employeesCount: number;
	normativeHours: number;
	fiftyPercentageBonusHours: number;
	hundredPercentageBonusHours: number;
	saturdayHours: number;
	nightHours: number;
}

export interface SalaryReportViewModel {
	employeesCount: number;
	grossBaseSalary: number;
	bonusSalary: number;
	overtimeSalary: number;
	saturdaySalary: number;
	nightSalary: number;
	holidaySalary: number;
	sicknessSalary: number;
	additionalSalary: number;
	compensationSalary: number;
	finalSalary: number;
}
