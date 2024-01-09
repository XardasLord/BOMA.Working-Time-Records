import { Injectable } from '@angular/core';
import { RemoteServiceBase } from '../../shared/services/remote.service.base';
import { HttpClient } from '@angular/common/http';
import { UserDetails } from '../../shared/auth/models/userDetails';
import { Observable } from 'rxjs';

@Injectable()
export class UsersService extends RemoteServiceBase {
	constructor(httpClient: HttpClient) {
		super(httpClient);
	}

	public getUsers(): Observable<UserDetails[]> {
		return this.httpClient.get<UserDetails[]>(`${this.apiEndpoint}/auth/users`);
	}

	public activate(userId: string) {
		return this.httpClient.patch(`${this.apiEndpoint}/auth/users/${userId}/activation`, {
			activationStatus: 1
		});
	}

	public deactivate(userId: string) {
		return this.httpClient.patch(`${this.apiEndpoint}/auth/users/${userId}/activation`, {
			activationStatus: 0
		});
	}
}
