import { AfterViewInit, Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngxs/store';
import { UpdateFormValue } from '@ngxs/form-plugin';
import { ToastrService } from 'ngx-toastr';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { WorkingTimeRecordState } from '../../state/working-time-record.state';
import { EmployeeWorkingTimeRecordDetailsModel } from '../../models/employee-working-time-record-details.model';
import { FormControlOriginalValueValidationModel } from '../../../shared/validators/form-control-original-value-validation.model';
import { createValueChangedValidator } from '../../../shared/helpers/forms.helper';
import { nameof } from '../../../shared/helpers/name-of.helper';
import { WorkingTimeRecordSummaryDataFormModel } from '../../models/working-time-record-summary-data-form.model';
import { WorkingTimeRecord } from '../../state/working-time-record.action';
import UpdateSummaryData = WorkingTimeRecord.UpdateSummaryData;
import { AuthState } from '../../../shared/auth/state/auth.state';

@Component({
	selector: 'app-working-time-record-summary-table',
	templateUrl: './working-time-record-summary-table.component.html',
	styleUrls: ['./working-time-record-summary-table.component.scss']
})
export class WorkingTimeRecordSummaryTableComponent implements AfterViewInit {
	detailedRecords$!: Observable<EmployeeWorkingTimeRecordDetailsModel[]>;

	salaryForm: FormGroup;
	editingRow: EmployeeWorkingTimeRecordDetailsModel | null = null;
	baseSalaryOriginalValue: FormControlOriginalValueValidationModel = {};
	percentageBonusSalaryOriginalValue: FormControlOriginalValueValidationModel = {};
	holidaySalaryOriginalValue: FormControlOriginalValueValidationModel = {};
	sicknessSalaryOriginalValue: FormControlOriginalValueValidationModel = {};
	additionalSalaryOriginalValue: FormControlOriginalValueValidationModel = {};

	columnsToDisplay = [
		'index',
		'fullName',
		'baseSalary',
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

	role$ = this.store.select(AuthState.getRole);

	constructor(
		private store: Store,
		private fb: FormBuilder,
		private toastService: ToastrService
	) {
		this.salaryForm = this.fb.group({
			employeeId: new FormControl<number>(0),
			baseSalary: new FormControl<number>(0, [
				Validators.required,
				Validators.min(1),
				createValueChangedValidator(this.baseSalaryOriginalValue)
			]),
			percentageBonusSalary: new FormControl<number>(0, [
				Validators.required,
				Validators.min(0),
				createValueChangedValidator(this.percentageBonusSalaryOriginalValue)
			]),
			holidaySalary: new FormControl<number>(0, [
				Validators.required,
				Validators.min(0),
				createValueChangedValidator(this.holidaySalaryOriginalValue)
			]),
			sicknessSalary: new FormControl<number>(0, [
				Validators.required,
				Validators.min(0),
				createValueChangedValidator(this.sicknessSalaryOriginalValue)
			]),
			additionalSalary: new FormControl<number>(0, [
				Validators.required,
				Validators.min(0),
				createValueChangedValidator(this.additionalSalaryOriginalValue)
			]),
			year: new FormControl<number>(0),
			month: new FormControl<number>(0)
		});
	}

	ngAfterViewInit() {
		this.detailedRecords$ = this.store.select(WorkingTimeRecordState.getDetailedRecords);
	}

	trackSummaryRecord(index: number, element: EmployeeWorkingTimeRecordDetailsModel): number {
		return element.employee.id;
	}

	isRowEditing(row: EmployeeWorkingTimeRecordDetailsModel): boolean {
		return row === this.editingRow;
	}

	enableEditMode(record: EmployeeWorkingTimeRecordDetailsModel) {
		if (!record.isEditable) {
			this.toastService.warning('Ten rekord nie jest edytowalny');
			return;
		}

		this.editingRow = record;
		this.baseSalaryOriginalValue.originalValue = record.salaryInformation.baseSalary;
		this.percentageBonusSalaryOriginalValue.originalValue = record.salaryInformation.percentageBonusSalary;
		this.holidaySalaryOriginalValue.originalValue = record.salaryInformation.holidaySalary;
		this.sicknessSalaryOriginalValue.originalValue = record.salaryInformation.sicknessSalary;
		this.additionalSalaryOriginalValue.originalValue = record.salaryInformation.additionalSalary;

		this.store.dispatch(
			new UpdateFormValue({
				path: 'workingTimeRecord.summaryForm',
				value: record.salaryInformation.baseSalary,
				propertyPath: nameof<WorkingTimeRecordSummaryDataFormModel>('baseSalary')
			})
		);
		this.store.dispatch(
			new UpdateFormValue({
				path: 'workingTimeRecord.summaryForm',
				value: record.salaryInformation.percentageBonusSalary,
				propertyPath: nameof<WorkingTimeRecordSummaryDataFormModel>('percentageBonusSalary')
			})
		);
		this.store.dispatch(
			new UpdateFormValue({
				path: 'workingTimeRecord.summaryForm',
				value: record.salaryInformation.holidaySalary,
				propertyPath: nameof<WorkingTimeRecordSummaryDataFormModel>('holidaySalary')
			})
		);
		this.store.dispatch(
			new UpdateFormValue({
				path: 'workingTimeRecord.summaryForm',
				value: record.salaryInformation.sicknessSalary,
				propertyPath: nameof<WorkingTimeRecordSummaryDataFormModel>('sicknessSalary')
			})
		);
		this.store.dispatch(
			new UpdateFormValue({
				path: 'workingTimeRecord.summaryForm',
				value: record.salaryInformation.additionalSalary,
				propertyPath: nameof<WorkingTimeRecordSummaryDataFormModel>('additionalSalary')
			})
		);
	}

	cancelEditMode() {
		this.editingRow = null;
	}

	onFormSubmit() {
		if (!this.salaryForm.valid) {
			this.toastService.warning('Formularz zawiera nieprawidłowe dane');
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

		this.store.dispatch(new UpdateSummaryData(updateModel.employeeId, updateModel));
		this.cancelEditMode();
	}

	getAllFinalSum() {
		return this.detailedRecords$?.pipe(
			map((results) => results.map((r) => r.salaryInformation).reduce((acc, obj) => acc + obj.finalSumSalary, 0))
		);
	}
}
