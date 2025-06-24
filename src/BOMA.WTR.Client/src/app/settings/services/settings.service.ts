import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RemoteServiceBase } from '../../shared/services/remote.service.base';
import { SettingModelRequest, SettingsModel } from '../models/settings.model';

@Injectable()
export class SettingsService extends RemoteServiceBase {
	constructor(httpClient: HttpClient) {
		super(httpClient);
	}

	public getSettings(): Observable<SettingsModel[]> {
		return this.httpClient.get<SettingsModel[]>(`${this.apiEndpoint}/settings`);
	}

	public updateSettings(settings: SettingModelRequest[]): Observable<void> {
		return this.httpClient.patch<void>(`${this.apiEndpoint}/settings`, { settings });
	}
}
