import { Injectable, NgZone } from '@angular/core';
import { MatLegacyDialog as MatDialog, MatLegacyDialogConfig as MatDialogConfig, MatLegacyDialogRef as MatDialogRef } from '@angular/material/legacy-dialog';
import { Action, State, StateContext, StateToken, Store } from '@ngxs/store';
import { Modal } from './modal.action';
import OpenAddNewEmployeeDialog = Modal.OpenAddNewEmployeeDialog;
import { AddNewEmployeeDialogComponent } from '../../employee/components/add-new-employee-dialog/add-new-employee-dialog.component';
import OpenEditEmployeeDialog = Modal.OpenEditEmployeeDialog;
import { EditEmployeeDialogComponent } from '../../employee/components/edit-employee-dialog/edit-employee-dialog.component';

export interface ModalStateModel {}

const MODAL_STATE_TOKEN = new StateToken<ModalStateModel>('modal');

@State<ModalStateModel>({
	name: MODAL_STATE_TOKEN
})
@Injectable()
export class ModalState {
	private addNewEmployeeDialogRef?: MatDialogRef<AddNewEmployeeDialogComponent>;
	private editEmployeeDialogRef?: MatDialogRef<EditEmployeeDialogComponent>;

	private readonly addNewEmployeeDialogConfig = new MatDialogConfig();
	private readonly editEmployeeDialogConfig = new MatDialogConfig();

	constructor(private zone: NgZone, private dialog: MatDialog, private store: Store) {
		this.addNewEmployeeDialogConfig = {
			width: '320px'
		};
		this.editEmployeeDialogConfig = this.addNewEmployeeDialogConfig;
	}

	@Action(OpenAddNewEmployeeDialog)
	openBatchStatusDialog(ctx: StateContext<ModalStateModel>, _: OpenAddNewEmployeeDialog) {
		this.closeDialog(this.addNewEmployeeDialogRef);

		return this.zone.run(
			() =>
				(this.addNewEmployeeDialogRef = this.dialog.open(AddNewEmployeeDialogComponent, {
					...this.addNewEmployeeDialogConfig
				}))
		);
	}

	@Action(OpenEditEmployeeDialog)
	openEditEmployeeDialog(ctx: StateContext<ModalStateModel>, action: OpenEditEmployeeDialog) {
		this.closeDialog(this.editEmployeeDialogRef);

		return this.zone.run(
			() =>
				(this.editEmployeeDialogRef = this.dialog.open(EditEmployeeDialogComponent, {
					...this.editEmployeeDialogConfig,
					data: action.employee
				}))
		);
	}

	private closeDialog<T>(dialogRef: MatDialogRef<T> | undefined): void {
		if (dialogRef) {
			dialogRef.close();
			dialogRef = undefined;
		}
	}
}
