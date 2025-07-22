export interface EmployeeModel {
	id: number;
	firstName: string;
	lastName: string;
	baseSalary: number;
	salaryBonusPercentage: number;
	rcpId: number;
	departmentId: number;
	departmentName: string;
	shiftTypeId: number | null;
	shiftTypeName: string;
	position: string;
	personalIdentityNumber: string;
	isActive: boolean;
}
