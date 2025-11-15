import { NgModule } from '@angular/core';
import { NgxsModule } from '@ngxs/store';
import { NgxsFormPluginModule } from '@ngxs/form-plugin';
import { SharedModule } from '../shared/shared.module';
import { WorkingTimeRecordComponent } from './components/working-time-record/working-time-record.component';
import { WorkingTimeRecordListComponent } from './components/working-time-record-list/working-time-record-list.component';
import { WorkingTimeRecordRoutingModule } from './working-time-record-routing.module';
import { WorkingTimeRecordState } from './state/working-time-record.state';
import { IWorkingTimeRecordService } from './services/working-time-record.service.base';
import { WorkingTimeRecordService } from './services/working-time-record.service';
import { WorkingTimeRecordFiltersComponent } from './components/working-time-record-filters/working-time-record-filters.component';
import { WorkingTimeRecordDetailedTableComponent } from './components/working-time-record-detailed-table/working-time-record-detailed-table.component';
import { WorkingTimeRecordSummaryTableComponent } from './components/working-time-record-summary-table/working-time-record-summary-table.component';
import { WorkingTimeRecordReportHoursTableComponent } from './components/working-time-record-report-hours-table/working-time-record-report-hours-table.component';
import { WorkingTimeRecordReportAbsentsTableComponent } from './components/working-time-record-report-absents-table/working-time-record-report-absents-table.component';
import { WorkingTimeRecordReportsComponent } from './components/working-time-record-report-hours-summary-table/working-time-record-reports.component';

@NgModule({
	declarations: [
		WorkingTimeRecordComponent,
		WorkingTimeRecordListComponent,
		WorkingTimeRecordFiltersComponent,
		WorkingTimeRecordDetailedTableComponent,
		WorkingTimeRecordSummaryTableComponent,
		WorkingTimeRecordReportHoursTableComponent,
		WorkingTimeRecordReportAbsentsTableComponent,
		WorkingTimeRecordReportsComponent
	],
	imports: [SharedModule, WorkingTimeRecordRoutingModule, NgxsModule.forFeature([WorkingTimeRecordState]), NgxsFormPluginModule],
	providers: [{ provide: IWorkingTimeRecordService, useClass: WorkingTimeRecordService }]
})
export class WorkingTimeRecordModule {}
