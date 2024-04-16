import { Component, OnInit } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { WorkingTimeRecord } from '../../state/working-time-record.action';
import GetAll = WorkingTimeRecord.GetAll;
import { Store } from '@ngxs/store';
import { AuthState } from '../../../shared/auth/state/auth.state';
import { Role } from '../../../shared/auth/models/userDetails';

@Component({
	selector: 'app-working-time-record-list',
	templateUrl: './working-time-record-list.component.html',
	styleUrls: ['./working-time-record-list.component.scss']
})
export class WorkingTimeRecordListComponent implements OnInit {
	Role = Role;

	userRole$ = this.store.select(AuthState.getUserRole);

	constructor(private store: Store) {}

	ngOnInit(): void {
		this.store.dispatch(new GetAll());
	}
}
