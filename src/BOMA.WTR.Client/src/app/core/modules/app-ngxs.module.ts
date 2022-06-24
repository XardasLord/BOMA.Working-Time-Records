import { NgModule } from '@angular/core';
import { NgxsModule } from '@ngxs/store';
import { environment } from '../../../environments/environment';
import { NgxsLoggerPluginModule } from '@ngxs/logger-plugin';
import { NgxsRouterPluginModule } from '@ngxs/router-plugin';
import { NgxsReduxDevtoolsPluginModule } from '@ngxs/devtools-plugin';

@NgModule({
  declarations: [],
  imports: [
    NgxsModule.forRoot([], {
      developmentMode: !environment.production,
    }),
    NgxsLoggerPluginModule.forRoot({
      collapsed: false,
      disabled: environment.production,
    }),
    NgxsReduxDevtoolsPluginModule.forRoot({
      disabled: environment.production,
      name: 'BOMA Working Time Records',
    }),
    NgxsRouterPluginModule.forRoot(),
  ],
  exports: [NgxsReduxDevtoolsPluginModule],
})
export class AppNgxsModule {}
