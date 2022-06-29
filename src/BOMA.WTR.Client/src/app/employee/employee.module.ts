import { NgModule } from '@angular/core';
import { NgxsModule } from '@ngxs/store';
import { EmployeeRoutingModule } from './employee-routing.module';
import { EmployeeComponent } from './components/employee/employee.component';
import { EmployeeListComponent } from './components/employee-list/employee-list.component';
import { IEmployeeService } from './services/employee.service.base';
import { EmployeeService } from './services/employee.service';
import { EmployeeState } from './state/employee.state';
import { SharedModule } from '../shared/shared.module';

@NgModule({
	declarations: [EmployeeComponent, EmployeeListComponent],
	imports: [SharedModule, EmployeeRoutingModule, NgxsModule.forFeature([EmployeeState])],
	providers: [{ provide: IEmployeeService, useClass: EmployeeService }]
})
export class EmployeeModule {}
