import { Injectable, NgZone } from '@angular/core';
import { MatDialog, MatDialogConfig, MatDialogRef } from '@angular/material/dialog';
import { Action, State, StateContext, StateToken, Store } from '@ngxs/store';
import { Modal } from './modal.action';
import OpenAddNewEmployeeDialog = Modal.OpenAddNewEmployeeDialog;
import { AddNewEmployeeDialogComponent } from '../../employee/components/add-new-employee-dialog/add-new-employee-dialog.component';

export interface ModalStateModel {}

const MODAL_STATE_TOKEN = new StateToken<ModalStateModel>('modal');

@State<ModalStateModel>({
	name: MODAL_STATE_TOKEN
})
@Injectable()
export class ModalState {
	private addNewEmployeeDialogRef?: MatDialogRef<AddNewEmployeeDialogComponent>;

	private readonly addNewEmployeeDialogConfig = new MatDialogConfig();

	constructor(private zone: NgZone, private dialog: MatDialog, private store: Store) {
		this.addNewEmployeeDialogConfig = {
			width: '320px'
		};
	}

	@Action(OpenAddNewEmployeeDialog)
	openBatchStatusDialog(ctx: StateContext<ModalStateModel>, _: OpenAddNewEmployeeDialog) {
		this.closeDialog(this.addNewEmployeeDialogRef);

		return this.zone.run(() =>
			this.dialog.open(AddNewEmployeeDialogComponent, {
				...this.addNewEmployeeDialogConfig
			})
		);
	}

	private closeDialog<T>(dialogRef: MatDialogRef<AddNewEmployeeDialogComponent> | undefined): void {
		if (dialogRef) {
			dialogRef.close();
			dialogRef = undefined;
		}
	}
}
