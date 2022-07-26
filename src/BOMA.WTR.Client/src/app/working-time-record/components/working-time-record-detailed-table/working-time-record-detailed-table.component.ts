import { AfterViewInit, Component } from '@angular/core';
import { Store } from '@ngxs/store';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import {
	WorkingTimeRecordDetailedDataFormModel,
	WorkingTimeRecordDetailedFormGroup
} from '../../models/working-time-record-summary-data-form.model';
import { WorkingTimeRecordState } from '../../state/working-time-record.state';
import { EmployeeWorkingTimeRecordDetailsModel } from '../../models/employee-working-time-record-details.model';
import { WorkingTimeRecordDetailsAggregatedModel } from '../../models/working-time-record-details-aggregated.model';
import { WorkingTimeRecord } from '../../state/working-time-record.action';
import GetAll = WorkingTimeRecord.GetAll;
import UpdateDetailedData = WorkingTimeRecord.UpdateDetailedData;

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
			year: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			month: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
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
		// Set initial value for all days
		this.detailedHoursForm.patchValue({
			employeeId: this.editingRow?.employee.id,
			day1: this.editingRow?.workingTimeRecordsAggregated[0]?.workedHoursRounded,
			day2: this.editingRow?.workingTimeRecordsAggregated[1]?.workedHoursRounded,
			day3: this.editingRow?.workingTimeRecordsAggregated[2]?.workedHoursRounded,
			day4: this.editingRow?.workingTimeRecordsAggregated[3]?.workedHoursRounded,
			day5: this.editingRow?.workingTimeRecordsAggregated[4]?.workedHoursRounded,
			day6: this.editingRow?.workingTimeRecordsAggregated[5]?.workedHoursRounded,
			day7: this.editingRow?.workingTimeRecordsAggregated[6]?.workedHoursRounded,
			day8: this.editingRow?.workingTimeRecordsAggregated[7]?.workedHoursRounded,
			day9: this.editingRow?.workingTimeRecordsAggregated[8]?.workedHoursRounded,
			day10: this.editingRow?.workingTimeRecordsAggregated[9]?.workedHoursRounded,
			day11: this.editingRow?.workingTimeRecordsAggregated[10]?.workedHoursRounded,
			day12: this.editingRow?.workingTimeRecordsAggregated[11]?.workedHoursRounded,
			day13: this.editingRow?.workingTimeRecordsAggregated[12]?.workedHoursRounded,
			day14: this.editingRow?.workingTimeRecordsAggregated[13]?.workedHoursRounded,
			day15: this.editingRow?.workingTimeRecordsAggregated[14]?.workedHoursRounded,
			day16: this.editingRow?.workingTimeRecordsAggregated[15]?.workedHoursRounded,
			day17: this.editingRow?.workingTimeRecordsAggregated[16]?.workedHoursRounded,
			day18: this.editingRow?.workingTimeRecordsAggregated[17]?.workedHoursRounded,
			day19: this.editingRow?.workingTimeRecordsAggregated[18]?.workedHoursRounded,
			day20: this.editingRow?.workingTimeRecordsAggregated[19]?.workedHoursRounded,
			day21: this.editingRow?.workingTimeRecordsAggregated[20]?.workedHoursRounded,
			day22: this.editingRow?.workingTimeRecordsAggregated[21]?.workedHoursRounded,
			day23: this.editingRow?.workingTimeRecordsAggregated[22]?.workedHoursRounded,
			day24: this.editingRow?.workingTimeRecordsAggregated[23]?.workedHoursRounded,
			day25: this.editingRow?.workingTimeRecordsAggregated[24]?.workedHoursRounded,
			day26: this.editingRow?.workingTimeRecordsAggregated[25]?.workedHoursRounded,
			day27: this.editingRow?.workingTimeRecordsAggregated[26]?.workedHoursRounded,
			day28: this.editingRow?.workingTimeRecordsAggregated[27]?.workedHoursRounded,
			day29: this.editingRow?.workingTimeRecordsAggregated[28]?.workedHoursRounded,
			day30: this.editingRow?.workingTimeRecordsAggregated[29]?.workedHoursRounded,
			day31: this.editingRow?.workingTimeRecordsAggregated[30]?.workedHoursRounded
		});
	}

	cancelEditMode() {
		this.editingRow = null;
	}

	onFormSubmit() {
		if (!this.detailedHoursForm.valid) {
			this.toastService.warning('Formularz zawiera nieprawidłowe dane');
			return;
		}

		const query = this.store.selectSnapshot(WorkingTimeRecordState.getSearchQueryModel);

		this.detailedHoursForm.patchValue({
			year: query.year,
			month: query.month
		});

		const updateModel = this.detailedHoursForm.getRawValue() as WorkingTimeRecordDetailedDataFormModel;

		this.store.dispatch(new UpdateDetailedData(updateModel.employeeId, updateModel));
		this.cancelEditMode();
	}
}
