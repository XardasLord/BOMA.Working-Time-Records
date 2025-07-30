import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogActions, MatDialogContent, MatDialogRef, MatDialogTitle } from '@angular/material/dialog';
import { MatButton } from '@angular/material/button';

@Component({
	selector: 'app-information-dialog',
	templateUrl: './information-dialog.component.html',
	styleUrls: ['./information-dialog.component.scss'],
	imports: [MatDialogTitle, MatDialogContent, MatDialogActions, MatButton],
	standalone: true
})
export class InformationDialogComponent {
	title: string;
	message: string;

	constructor(
		public dialogRef: MatDialogRef<InformationDialogComponent>,
		@Inject(MAT_DIALOG_DATA) public data: InformationDialogModel
	) {
		this.title = data.title;
		this.message = data.message;
	}

	onClose(): void {
		this.dialogRef.close();
	}
}

export class InformationDialogModel {
	constructor(
		public title: string,
		public message: string
	) {}
}
