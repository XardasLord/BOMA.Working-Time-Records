export namespace Users {
	const prefix = '[Users]';

	export class GetAll {
		static readonly type = `${prefix} ${GetAll.name}`;
	}

	export class Activate {
		constructor(public userId: string) {}

		static readonly type = `${prefix} ${Activate.name}`;
	}

	export class Deactivate {
		constructor(public userId: string) {}

		static readonly type = `${prefix} ${Deactivate.name}`;
	}
}
