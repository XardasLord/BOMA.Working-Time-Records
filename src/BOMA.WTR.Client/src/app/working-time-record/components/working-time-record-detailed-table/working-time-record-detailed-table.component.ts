import { AfterViewInit, Component } from '@angular/core';
import { Store } from '@ngxs/store';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { WorkingTimeRecordState } from '../../state/working-time-record.state';
import { EmployeeWorkingTimeRecordDetailsModel } from '../../models/employee-working-time-record-details.model';
import { WorkingTimeRecordDetailsAggregatedModel } from '../../models/working-time-record-details-aggregated.model';
import { WorkingTimeRecord } from '../../state/working-time-record.action';
import GetAll = WorkingTimeRecord.GetAll;
import { UpdateFormValue } from '@ngxs/form-plugin';
import { nameof } from '../../../shared/helpers/name-of.helper';
import {
	WorkingTimeRecordDetailedDataFormModel,
	WorkingTimeRecordDetailedFormGroup
} from '../../models/working-time-record-summary-data-form.model';

@Component({
	selector: 'app-working-time-record-detailed-table',
	templateUrl: './working-time-record-detailed-table.component.html',
	styleUrls: ['./working-time-record-detailed-table.component.scss']
})
export class WorkingTimeRecordDetailedTableComponent implements AfterViewInit {
	detailedRecords$ = this.store.select(WorkingTimeRecordState.getDetailedRecordsNormalizedForTable);
	numberOfDays$ = this.store.select(WorkingTimeRecordState.getDaysInMonth);
	columnsToDisplay$ = this.store.select(WorkingTimeRecordState.getColumnsToDisplay);

	detailedHoursForm: FormGroup<WorkingTimeRecordDetailedFormGroup>;
	editingRow: EmployeeWorkingTimeRecordDetailsModel | null = null;

	constructor(private store: Store, private fb: FormBuilder, private toastService: ToastrService) {
		this.detailedHoursForm = fb.group<WorkingTimeRecordDetailedFormGroup>({
			employeeId: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day1: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day2: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day3: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day4: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day5: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day6: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day7: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day8: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day9: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day10: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day11: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day12: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day13: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day14: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day15: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day16: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day17: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day18: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day19: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day20: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day21: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day22: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day23: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day24: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day25: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day26: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day27: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day28: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day29: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day30: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day31: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] })
		});
	}

	ngAfterViewInit() {
		this.store.dispatch(new GetAll());
	}

	trackDetailedRecord(index: number, element: EmployeeWorkingTimeRecordDetailsModel): number {
		return element.employee.id;
	}

	// isWeekendDay(workingTimeRecordDetails: WorkingTimeRecordDetailsAggregatedModel[], dayOfMonth: number): boolean {
	// 	const recordFromDay = workingTimeRecordDetails.filter((x) => new Date(x.date).getDate() === dayOfMonth);
	// 	return recordFromDay[0]?.isWeekendDay ?? true;
	// }

	getWorkingHoursSum(workingTimeRecordDetails: WorkingTimeRecordDetailsAggregatedModel[]) {
		return workingTimeRecordDetails.reduce<number>((accumulator, obj) => accumulator + obj.workedHoursRounded, 0);
	}

	getNormativeHoursSum(workingTimeRecordDetails: WorkingTimeRecordDetailsAggregatedModel[]) {
		return workingTimeRecordDetails.reduce<number>((accumulator, obj) => accumulator + obj.baseNormativeHours, 0);
	}

	get50PercentageHoursSum(workingTimeRecordDetails: WorkingTimeRecordDetailsAggregatedModel[]) {
		return workingTimeRecordDetails.reduce<number>((accumulator, obj) => accumulator + obj.fiftyPercentageBonusHours, 0);
	}

	get100PercentageHoursSum(workingTimeRecordDetails: WorkingTimeRecordDetailsAggregatedModel[]) {
		return workingTimeRecordDetails.reduce<number>((accumulator, obj) => accumulator + obj.hundredPercentageBonusHours, 0);
	}

	getSaturdayHoursSum(workingTimeRecordDetails: WorkingTimeRecordDetailsAggregatedModel[]) {
		return workingTimeRecordDetails.reduce<number>((accumulator, obj) => accumulator + obj.saturdayHours, 0);
	}

	getNightHoursSum(workingTimeRecordDetails: WorkingTimeRecordDetailsAggregatedModel[]) {
		return workingTimeRecordDetails.reduce<number>((accumulator, obj) => accumulator + obj.nightHours, 0);
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
		// TODO: There is an issue with NGXS auto update form value - https://github.com/ngxs/store/issues/910
		// TODO: Set initial value for all days
		this.detailedHoursForm.patchValue({
			employeeId: this.editingRow?.employee.id
		});

		console.warn(this.detailedHoursForm.getRawValue());
	}

	cancelEditMode() {
		this.editingRow = null;
	}

	onFormSubmit() {
		console.warn('SUBMIT');
		if (!this.detailedHoursForm.valid) {
			this.toastService.warning('Formularz zawiera nieprawidłowe dane');
			return;
		}

		// TODO: Save
		this.cancelEditMode();
		this.toastService.success('Dane zostały zapisane');
	}
}
