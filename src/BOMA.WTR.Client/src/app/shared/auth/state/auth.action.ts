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

	export class GetMyAccountDetails {
		static readonly type = `${prefix} ${GetMyAccountDetails.name}`;
	}
}
