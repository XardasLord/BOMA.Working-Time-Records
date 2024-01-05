import { NgModule } from '@angular/core';
import { NgxsModule } from '@ngxs/store';
import { UsersComponent } from './components/users/users.component';
import { UsersListComponent } from './components/users-list/users-list.component';
import { SharedModule } from '../shared/shared.module';
import { UsersRoutingModule } from './users-routing.module';
import { UsersState } from './state/users.state';
import { UsersService } from './services/users.service';

@NgModule({
	declarations: [UsersComponent, UsersListComponent],
	imports: [SharedModule, UsersRoutingModule, NgxsModule.forFeature([UsersState])],
	providers: [UsersService]
})
export class UsersModule {}
