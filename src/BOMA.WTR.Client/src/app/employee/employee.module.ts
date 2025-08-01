import { NgModule } from '@angular/core';
import { NgxsModule } from '@ngxs/store';
import { NgxsFormPluginModule } from '@ngxs/form-plugin';
import { EmployeeRoutingModule } from './employee-routing.module';
import { EmployeeComponent } from './components/employee/employee.component';
import { EmployeeListComponent } from './components/employee-list/employee-list.component';
import { IEmployeeService } from './services/employee.service.base';
import { EmployeeService } from './services/employee.service';
import { EmployeeState } from './state/employee.state';
import { SharedModule } from '../shared/shared.module';
import { AddNewEmployeeDialogComponent } from './components/add-new-employee-dialog/add-new-employee-dialog.component';
import { EditEmployeeDialogComponent } from './components/edit-employee-dialog/edit-employee-dialog.component';
import { EmployeeFiltersComponent } from './components/employee-filters/employee-filters.component';

@NgModule({
	declarations: [EmployeeComponent, EmployeeListComponent, AddNewEmployeeDialogComponent, EditEmployeeDialogComponent],
	imports: [SharedModule, EmployeeRoutingModule, NgxsModule.forFeature([EmployeeState]), NgxsFormPluginModule, EmployeeFiltersComponent],
	providers: [{ provide: IEmployeeService, useClass: EmployeeService }]
})
export class EmployeeModule {}
