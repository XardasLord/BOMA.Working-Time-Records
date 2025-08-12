import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButton } from '@angular/material/button';
import { SharedModule } from '../../../shared/shared.module';
import { FormControl, FormGroup, Validators } from '@angular/forms';

type DateRangeForm = FormGroup<{
	start: FormControl<Date | null>;
	end: FormControl<Date | null>;
}>;

@Component({
	selector: 'app-send-hours-dialog',
	standalone: true,
	imports: [MatDialogModule, MatButton, SharedModule],
	templateUrl: './send-hours-dialog.component.html',
	styleUrls: ['./send-hours-dialog.component.scss']
})
export class SendHoursDialogComponent {
	title: string;
	message: string;

	today = this.stripTime(new Date());
	firstOfMonth = new Date(this.data.year, this.data.month - 1, 1);
	range: DateRangeForm;

	constructor(
		public dialogRef: MatDialogRef<SendHoursDialogComponent>,
		@Inject(MAT_DIALOG_DATA) public data: SendHoursDialogModel
	) {
		this.title = data.title;
		this.message = data.message;

		this.range = new FormGroup({
			start: new FormControl<Date | null>(this.firstOfMonth, { nonNullable: false }),
			end: new FormControl<Date | null>(this.today, { nonNullable: false })
		});

		this.range.controls.start.addValidators(Validators.required);
		this.range.controls.end.addValidators(Validators.required);

		// Walidacje: end >= start, end <= today
		this.range.addValidators(() => {
			const start = this.range.controls.start.value;
			const end = this.range.controls.end.value;

			if (start && end && end < start) return { endBeforeStart: true };
			if (end && end > this.today) return { endAfterToday: true };
			return null;
		});

		// gdy zmienia się start — przelicz walidację end
		this.range.controls.start.valueChanges.subscribe(() => {
			this.range.controls.end.updateValueAndValidity();
		});
	}

	setThisMonth(): void {
		const firstOfMonth = new Date(this.today.getFullYear(), this.today.getMonth(), 1);
		this.range.setValue({ start: firstOfMonth, end: this.today });
		this.range.updateValueAndValidity();
	}

	clearRange(): void {
		this.range.reset();
	}

	onConfirm(): void {
		if (!this.range.valid) {
			return;
		}

		const start = this.stripTime(this.range.controls.start.value!);
		const end = this.stripTime(this.range.controls.end.value!);

		this.dialogRef.close(new SendHoursDialogResponse(start, end));
	}

	onDismiss(): void {
		this.dialogRef.close(null);
	}

	private stripTime(d: Date): Date {
		return new Date(d.getFullYear(), d.getMonth(), d.getDate());
	}
}

export class SendHoursDialogModel {
	constructor(
		public title: string,
		public message: string,
		public month: number,
		public year: number
	) {}
}

export class SendHoursDialogResponse {
	constructor(
		public start: Date,
		public end: Date
	) {}
}
