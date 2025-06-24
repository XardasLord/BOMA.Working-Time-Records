import { SettingModelRequest } from '../models/settings.model';

export namespace Settings {
	const prefix = '[Settings]';

	export class GetAll {
		static readonly type = `${prefix} ${GetAll.name}`;
	}

	export class SaveMinimumWage {
		static readonly type = `${prefix} ${SaveMinimumWage.name}`;

		constructor(public minimumWage: number) {}
	}

	export class UpdateSettings {
		static readonly type = `${prefix} ${UpdateSettings.name}`;

		constructor(public settings: SettingModelRequest[]) {}
	}
}
