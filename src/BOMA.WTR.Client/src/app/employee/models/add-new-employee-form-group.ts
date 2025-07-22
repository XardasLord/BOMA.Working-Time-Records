import { FormControl } from '@angular/forms';

export interface AddNewEmployeeFormGroup {
	firstName: FormControl<string>;
	lastName: FormControl<string>;
	baseSalary: FormControl<number>;
	salaryBonusPercentage: FormControl<number>;
	rcpId: FormControl<number>;
	departmentId: FormControl<number>;
	departmentName: FormControl<string | null>;
	shiftTypeId: FormControl<number | null>;
	shiftTypeName: FormControl<string | null>;
	position: FormControl<string>;
	personalIdentityNumber: FormControl<string>;
}
