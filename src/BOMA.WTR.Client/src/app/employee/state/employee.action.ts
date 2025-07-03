import { AddNewEmployeeFormModel } from '../models/add-new-employee-form.model';
import { EditEmployeeFormModel } from '../models/edit-employee-form.model';

export namespace Employee {
	const prefix = '[Employee]';

	export class GetAll {
		static readonly type = `${prefix} ${GetAll.name}`;
	}

	export class ApplyFilter {
		constructor(public searchPhrase: string) {}

		static readonly type = `${prefix} ${ApplyFilter.name}`;
	}

	export class ChangeGroup {
		constructor(public groupId: number) {}

		static readonly type = `${prefix} ${ChangeGroup.name}`;
	}

	export class ChangeShift {
		constructor(public shiftId: number) {}

		static readonly type = `${prefix} ${ChangeShift.name}`;
	}

	export class Add {
		constructor(public employee: AddNewEmployeeFormModel) {}

		static readonly type = `${prefix} ${Add.name}`;
	}

	export class Edit {
		constructor(
			public employeeId: number,
			public employee: EditEmployeeFormModel
		) {}

		static readonly type = `${prefix} ${Edit.name}`;
	}

	export class Deactivate {
		constructor(public employeeId: number) {}

		static readonly type = `${prefix} ${Deactivate.name}`;
	}
}
