import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NavigationComponent } from '../ui/components/navigation/navigation.component';

export const RoutePaths = {
  Employees: 'employees',
  Groups: 'groups',
};

const routes: Routes = [
  {
    path: '',
    component: NavigationComponent,
    children: [
      {
        path: RoutePaths.Employees,
        loadChildren: () =>
          import('../../employee/employee.module').then(
            (m) => m.EmployeeModule
          ),
      },
    ],
  },
  {
    path: '**',
    redirectTo: '',
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
