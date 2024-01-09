import { HttpClient } from '@angular/common/http';
import { RemoteServiceBase } from '../../services/remote.service.base';
import { Injectable } from '@angular/core';
import { RegisterCommand } from '../models/register.command';
import { RegisterResponse } from '../models/register.response';
import { Observable, of } from 'rxjs';
import { LoginCommand } from '../models/login.command';
import { LoginResponse } from '../models/login.response';
import { UserDetails } from '../models/userDetails';

@Injectable()
export class AuthService extends RemoteServiceBase {
	constructor(httpClient: HttpClient) {
		super(httpClient);
	}

	public register(command: RegisterCommand): Observable<RegisterResponse> {
		return this.httpClient.post<RegisterResponse>(`${this.identityEndpoint}/register`, command);
	}

	public login(command: LoginCommand): Observable<LoginResponse> {
		return this.httpClient.post<LoginResponse>(`${this.identityEndpoint}/login`, command);
	}

	public logout() {
		return of({});
	}

	public getUsers(): Observable<UserDetails[]> {
		return this.httpClient.get<UserDetails[]>(`${this.apiEndpoint}/auth/users`);
	}

	public getMyAccountDetails(): Observable<UserDetails> {
		return this.httpClient.get<UserDetails>(`${this.apiEndpoint}/auth/me`);
	}
}
