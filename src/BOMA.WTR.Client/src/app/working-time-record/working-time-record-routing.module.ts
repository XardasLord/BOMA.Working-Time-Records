import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { WorkingTimeRecordListComponent } from './components/working-time-record-list/working-time-record-list.component';
import { WorkingTimeRecordComponent } from './components/working-time-record/working-time-record.component';

const routes: Routes = [
	{
		path: '',
		component: WorkingTimeRecordComponent,
		children: [
			{
				path: '',
				component: WorkingTimeRecordListComponent
			}
		]
	}
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule]
})
export class WorkingTimeRecordRoutingModule {}
