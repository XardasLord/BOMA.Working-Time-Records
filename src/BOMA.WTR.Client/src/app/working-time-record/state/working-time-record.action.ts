export namespace WorkingTimeRecord {
	const prefix = '[WorkingTimeRecord]';

	export class GetAll {
		constructor(public year: number, public month: number, public groupId: number) {}

		static readonly type = `${prefix} ${GetAll.name}`;
	}
}
