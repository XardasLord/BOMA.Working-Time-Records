import { FormControl } from '@angular/forms';

export interface EditEmployeeFormGroup {
	id: FormControl<number>;
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
}
