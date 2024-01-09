import packageJson from '../../package.json';

export const environment = {
	production: true,
	appVersion: `${packageJson.version}`,
	identityEndpoint: 'http://192.168.0.161:81',
	apiEndpoint: 'http://192.168.0.161:81/api',
	managerMode: false
};
