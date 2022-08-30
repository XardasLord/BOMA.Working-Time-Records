import { Component } from '@angular/core';
import { environment } from '../../../../environments/environment';

@Component({
	selector: 'app-working-time-record-list',
	templateUrl: './working-time-record-list.component.html',
	styleUrls: ['./working-time-record-list.component.scss']
})
export class WorkingTimeRecordListComponent {
	managerMode = environment.managerMode;
}
