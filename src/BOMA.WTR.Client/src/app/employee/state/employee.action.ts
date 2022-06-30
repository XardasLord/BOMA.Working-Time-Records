import { AddNewEmployeeFormModel } from '../models/add-new-employee-form.model';

export namespace Employee {
	const prefix = '[Employee]';

	export class GetAll {
		static readonly type = `${prefix} ${GetAll.name}`;
	}

	export class Add {
		constructor(public employee: AddNewEmployeeFormModel) {}

		static readonly type = `${prefix} ${Add.name}`;
	}
}
