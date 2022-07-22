import { Component } from '@angular/core';
import { Store } from '@ngxs/store';
import { UpdateFormValue } from '@ngxs/form-plugin';
import { WorkingTimeRecordState } from '../../state/working-time-record.state';
import { EmployeeWorkingTimeRecordDetailsModel } from '../../models/employee-working-time-record-details.model';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { FormControlOriginalValueValidationModel } from '../../models/FormControlOriginalValueValidationModel';
import { createValueChangedValidator } from '../../../shared/helpers/forms.helper';
import { nameof } from '../../../shared/helpers/name-of.helper';
import { WorkingTimeRecordSummaryDataFormModel } from '../../models/working-time-record-summary-data-form.model';
import { WorkingTimeRecord } from '../../state/working-time-record.action';
import UpdateSummaryData = WorkingTimeRecord.UpdateSummaryData;
import { QueryModel } from '../../models/query.model';

@Component({
	selector: 'app-working-time-record-summary-table',
	templateUrl: './working-time-record-summary-table.component.html',
	styleUrls: ['./working-time-record-summary-table.component.scss']
})
export class WorkingTimeRecordSummaryTableComponent {
	detailedRecords$ = this.store.select(WorkingTimeRecordState.getDetailedRecords);

	editingRow: EmployeeWorkingTimeRecordDetailsModel | null = null;
	holidaySalaryOriginalValue: FormControlOriginalValueValidationModel = {};
	salaryForm: FormGroup;

	columnsToDisplay = [
		'fullName',
		'rate',
		'bonusPercentage',
		'gross',
		'bonus',
		'overtimes',
		'saturdays',
		'night',
		'nightHours',
		'holiday',
		'sickness',
		'additional',
		'sum',
		'actions'
	];

	constructor(private store: Store, private fb: FormBuilder) {
		this.salaryForm = fb.group({
			employeeId: new FormControl<number>(0),
			holidaySalary: new FormControl<number>(0, [
				Validators.required,
				Validators.min(0),
				createValueChangedValidator(this.holidaySalaryOriginalValue)
			]),
			year: new FormControl<number>(0),
			month: new FormControl<number>(0)
		});
	}

	isRowEditing(row: EmployeeWorkingTimeRecordDetailsModel): boolean {
		return row === this.editingRow;
	}

	enableEditMode(record: EmployeeWorkingTimeRecordDetailsModel) {
		if (!record.isEditable) {
			console.log('This record cannot be edited');
			return;
		}

		this.editingRow = record;
		this.holidaySalaryOriginalValue.originalValue = record.salaryInformation.holidaySalary;

		this.store.dispatch(
			new UpdateFormValue({
				path: 'workingTimeRecord.summaryForm',
				value: record.salaryInformation.holidaySalary ?? 0,
				propertyPath: nameof<WorkingTimeRecordSummaryDataFormModel>('holidaySalary')
			})
		);
		// TODO: Update rest of form values
	}

	cancelEditMode() {
		this.editingRow = null;
	}

	onFormSubmit() {
		if (!this.salaryForm.valid) {
			return;
		}

		this.store.dispatch(
			new UpdateFormValue({
				path: 'workingTimeRecord.summaryForm',
				value: this.editingRow?.employee.id,
				propertyPath: nameof<WorkingTimeRecordSummaryDataFormModel>('employeeId')
			})
		);

		this.store.dispatch(
			new UpdateFormValue({
				path: 'workingTimeRecord.summaryForm',
				value: this.store.selectSnapshot(WorkingTimeRecordState.getSearchQueryModel).year,
				propertyPath: nameof<WorkingTimeRecordSummaryDataFormModel>('year')
			})
		);

		this.store.dispatch(
			new UpdateFormValue({
				path: 'workingTimeRecord.summaryForm',
				value: this.store.selectSnapshot(WorkingTimeRecordState.getSearchQueryModel).month,
				propertyPath: nameof<WorkingTimeRecordSummaryDataFormModel>('month')
			})
		);

		const updateModel = this.salaryForm.getRawValue() as WorkingTimeRecordSummaryDataFormModel;

		// TODO: Save changes
		this.store.dispatch(new UpdateSummaryData(updateModel.employeeId, updateModel));
		this.cancelEditMode();
	}
}
