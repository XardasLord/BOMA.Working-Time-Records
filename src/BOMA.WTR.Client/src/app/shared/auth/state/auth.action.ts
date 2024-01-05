import { RegisterCommand } from '../models/register.command';
import { LoginCommand } from '../models/login.command';

export namespace Auth {
	const prefix = '[Auth]';

	export class Register {
		constructor(public command: RegisterCommand) {}

		static readonly type = `${prefix} ${Register.name}`;
	}

	export class Login {
		constructor(public command: LoginCommand) {}

		static readonly type = `${prefix} ${Login.name}`;
	}

	export class Logout {
		static readonly type = `${prefix} ${Logout.name}`;
	}

	export class Activate {
		constructor(public userId: string) {}

		static readonly type = `${prefix} ${Activate.name}`;
	}

	export class Deactivate {
		constructor(public userId: string) {}

		static readonly type = `${prefix} ${Deactivate.name}`;
	}

	export class GetUsers {
		static readonly type = `${prefix} ${GetUsers.name}`;
	}

	export class GetMyRole {
		static readonly type = `${prefix} ${GetMyRole.name}`;
	}
}
