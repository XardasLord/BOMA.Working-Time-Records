import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NavigationComponent } from '../ui/components/navigation/navigation.component';
import { Role } from '../../shared/auth/models/userDetails';
import { HasRoleGuard } from '../guards/has-role.guard';

export const RoutePaths = {
	Employees: 'employees',
	Groups: 'groups',
	WorkingTimeRecords: 'working-time-records',
	Users: 'users',
	Auth: 'auth',
	Register: 'auth/register',
	Login: 'auth/login'
};

const routes: Routes = [
	{
		path: '',
		component: NavigationComponent,
		children: [
			{
				path: RoutePaths.Employees,
				loadChildren: () => import('../../employee/employee.module').then((m) => m.EmployeeModule)
			},
			{
				path: RoutePaths.WorkingTimeRecords,
				loadChildren: () => import('../../working-time-record/working-time-record.module').then((m) => m.WorkingTimeRecordModule)
			},
			{
				path: RoutePaths.Users,
				loadChildren: () => import('../../users/users.module').then((m) => m.UsersModule),
				canActivate: [HasRoleGuard],
				data: {
					roles: [Role.Admin]
				}
			},
			{
				path: RoutePaths.Auth,
				loadChildren: () => import('../../shared/auth/auth.module').then((m) => m.AuthModule)
			}
		]
	},
	{
		path: '**',
		redirectTo: ''
	}
];

@NgModule({
	imports: [RouterModule.forRoot(routes)],
	exports: [RouterModule]
})
export class AppRoutingModule {}
