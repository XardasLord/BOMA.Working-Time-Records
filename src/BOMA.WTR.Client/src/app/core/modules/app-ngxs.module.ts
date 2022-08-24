import { NgModule } from '@angular/core';
import { NgxsModule } from '@ngxs/store';
import { environment } from '../../../environments/environment';
import { NgxsLoggerPluginModule } from '@ngxs/logger-plugin';
import { NgxsRouterPluginModule } from '@ngxs/router-plugin';
import { NgxsReduxDevtoolsPluginModule } from '@ngxs/devtools-plugin';
import { ModalState } from '../../shared/state/modal.state';
import { NgxsFormPluginModule } from '@ngxs/form-plugin';

@NgModule({
	declarations: [],
	imports: [
		NgxsModule.forRoot([ModalState], {
			developmentMode: !environment.production
		}),
		NgxsLoggerPluginModule.forRoot({
			collapsed: false,
			disabled: environment.production
		}),
		NgxsReduxDevtoolsPluginModule.forRoot({
			disabled: environment.production,
			name: 'BOMA Working Time Records'
		}),
		NgxsRouterPluginModule.forRoot(),
		NgxsFormPluginModule.forRoot()
	],
	exports: [NgxsReduxDevtoolsPluginModule]
})
export class AppNgxsModule {}
