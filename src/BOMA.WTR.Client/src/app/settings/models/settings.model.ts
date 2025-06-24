export interface SettingsModel {
	key: string;
	value: string;
	type: 'int' | 'string';
	description: string;
	lastModified: string;
}

export interface SettingModelRequest {
	key: string;
	value: string;
}
