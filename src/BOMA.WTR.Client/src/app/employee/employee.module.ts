import { NgModule } from '@angular/core';
import { EmployeeRoutingModule } from './employee-routing.module';
import { EmployeeComponent } from './components/employee/employee.component';
import { EmployeeListComponent } from './components/employee-list/employee-list.component';
import { CommonModule } from '@angular/common';
import { IEmployeeService } from './services/employee.service.base';
import { EmployeeService } from './services/employee.service';
import { NgxsModule } from '@ngxs/store';
import { EmployeeState } from './state/employee.state';

@NgModule({
  declarations: [EmployeeComponent, EmployeeListComponent],
  imports: [
    EmployeeRoutingModule,
    CommonModule,
    NgxsModule.forFeature([EmployeeState]),
  ],
  providers: [{ provide: IEmployeeService, useClass: EmployeeService }],
})
export class EmployeeModule {}
