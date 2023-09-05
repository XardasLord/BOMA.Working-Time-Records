import packageJson from '../../package.json';

export const environment = {
	production: true,
	appVersion: `${packageJson.version}-szef`,
	apiEndpoint: 'http://192.168.0.161:91/api',
	managerMode: false
};
