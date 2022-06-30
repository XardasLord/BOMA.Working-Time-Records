import { EmployeeModel } from '../../employee/models/employee.model';

export namespace Modal {
	const prefix = '[Modal]';

	export class OpenAddNewEmployeeDialog {
		static readonly type = `${prefix} ${OpenAddNewEmployeeDialog.name}`;
	}

	export class OpenEditEmployeeDialog {
		constructor(public employee: EmployeeModel) {}

		static readonly type = `${prefix} ${OpenEditEmployeeDialog.name}`;
	}
}
