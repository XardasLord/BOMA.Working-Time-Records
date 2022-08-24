import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Injectable } from '@angular/core';
import { IProgressSpinnerService } from './progress-spinner.base.service';
import { ProgressSpinnerDialogComponent } from '../ui/components/progress-spinner/progress-spinner-dialog.component';

@Injectable({
	providedIn: 'root'
})
export class ProgressSpinnerService implements IProgressSpinnerService {
	private progressSpinnerDialogRef?: MatDialogRef<ProgressSpinnerDialogComponent>;

	hideProgressSpinner(): void {
		if (this.progressSpinnerDialogRef) {
			this.progressSpinnerDialogRef.close();

			this.progressSpinnerDialogRef = undefined;
		}
	}

	showProgressSpinner(): void {
		this.hideProgressSpinner();
		this.progressSpinnerDialogRef = this.dialog.open(ProgressSpinnerDialogComponent, {
			panelClass: 'transparent',

			disableClose: true
		});
	}

	constructor(private dialog: MatDialog) {}
}
