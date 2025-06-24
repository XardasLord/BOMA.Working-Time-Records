import { inject, Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';

import { catchError, finalize, Observable, tap, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

import { IProgressSpinnerService } from '../../shared/services/progress-spinner.base.service';
import { SettingsModel } from '../models/settings.model';
import { SettingsService } from '../services/settings.service';
import { Settings } from './settings.action';
import GetAll = Settings.GetAll;
import SaveMinimumWage = Settings.SaveMinimumWage;
import UpdateSettings = Settings.UpdateSettings;

export interface SettingsStateModel {
	settings: SettingsModel[];
}

const SETTINGS_STATE_TOKEN = new StateToken<SettingsStateModel>('settings');
@State<SettingsStateModel>({
	name: SETTINGS_STATE_TOKEN,
	defaults: {
		settings: []
	}
})
@Injectable()
export class SettingsState {
	private settingsService = inject(SettingsService);
	private toastService = inject(ToastrService);
	private progressSpinnerService = inject(IProgressSpinnerService);

	@Selector([SETTINGS_STATE_TOKEN])
	static getSettings(state: SettingsStateModel): SettingsModel[] {
		return state.settings;
	}

	@Action(GetAll)
	getAll(ctx: StateContext<SettingsStateModel>, _: GetAll): Observable<SettingsModel[]> {
		this.progressSpinnerService.showProgressSpinner();

		return this.settingsService.getSettings().pipe(
			tap((response) => {
				ctx.patchState({
					settings: response
				});
			}),
			catchError((e) => {
				return throwError(e);
			}),
			finalize(() => {
				this.progressSpinnerService.hideProgressSpinner();
			})
		);
	}

	@Action(UpdateSettings)
	updateSettings(ctx: StateContext<SettingsStateModel>, action: UpdateSettings): Observable<void> {
		return this.settingsService.updateSettings(action.settings).pipe(
			tap((_) => {
				this.toastService.success(`Ustawienia zostały zapisane`, 'Sukces');
			}),
			catchError((e: HttpErrorResponse) => {
				this.toastService.error(`Wystąpił błąd przy aktualizacji ustawień - ${e.message}`, 'Błąd');
				return throwError(() => e);
			})
		);
	}
}
