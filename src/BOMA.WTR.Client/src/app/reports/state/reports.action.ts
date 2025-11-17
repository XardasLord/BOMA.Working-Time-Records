export namespace Reports {
	const prefix = '[Reports]';

	export class LoadData {
		constructor() {}

		static readonly type = `${prefix} ${LoadData.name}`;
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

	export class ChangeDate {
		constructor(
			public year: number,
			public month: number
		) {}

		static readonly type = `${prefix} ${ChangeDate.name}`;
	}
}
