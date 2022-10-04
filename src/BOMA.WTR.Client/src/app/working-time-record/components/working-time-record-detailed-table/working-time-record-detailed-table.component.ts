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
import PrintData = WorkingTimeRecord.PrintData;

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
			day31: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),

			day1saturday: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day2saturday: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day3saturday: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day4saturday: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day5saturday: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day6saturday: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day7saturday: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day8saturday: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day9saturday: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day10saturday: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day11saturday: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day12saturday: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day13saturday: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day14saturday: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day15saturday: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day16saturday: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day17saturday: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day18saturday: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day19saturday: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day20saturday: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day21saturday: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day22saturday: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day23saturday: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day24saturday: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day25saturday: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day26saturday: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day27saturday: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day28saturday: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day29saturday: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day30saturday: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day31saturday: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),

			day1night: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day2night: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day3night: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day4night: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day5night: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day6night: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day7night: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day8night: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day9night: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day10night: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day11night: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day12night: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day13night: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day14night: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day15night: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day16night: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day17night: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day18night: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day19night: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day20night: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day21night: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day22night: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day23night: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day24night: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day25night: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day26night: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day27night: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day28night: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day29night: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day30night: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day31night: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] })
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
			day31: this.editingRow?.workingTimeRecordsAggregated[30]?.workedHoursRounded,

			day1saturday: this.editingRow?.workingTimeRecordsAggregated[0]?.saturdayHours,
			day2saturday: this.editingRow?.workingTimeRecordsAggregated[1]?.saturdayHours,
			day3saturday: this.editingRow?.workingTimeRecordsAggregated[2]?.saturdayHours,
			day4saturday: this.editingRow?.workingTimeRecordsAggregated[3]?.saturdayHours,
			day5saturday: this.editingRow?.workingTimeRecordsAggregated[4]?.saturdayHours,
			day6saturday: this.editingRow?.workingTimeRecordsAggregated[5]?.saturdayHours,
			day7saturday: this.editingRow?.workingTimeRecordsAggregated[6]?.saturdayHours,
			day8saturday: this.editingRow?.workingTimeRecordsAggregated[7]?.saturdayHours,
			day9saturday: this.editingRow?.workingTimeRecordsAggregated[8]?.saturdayHours,
			day10saturday: this.editingRow?.workingTimeRecordsAggregated[9]?.saturdayHours,
			day11saturday: this.editingRow?.workingTimeRecordsAggregated[10]?.saturdayHours,
			day12saturday: this.editingRow?.workingTimeRecordsAggregated[11]?.saturdayHours,
			day13saturday: this.editingRow?.workingTimeRecordsAggregated[12]?.saturdayHours,
			day14saturday: this.editingRow?.workingTimeRecordsAggregated[13]?.saturdayHours,
			day15saturday: this.editingRow?.workingTimeRecordsAggregated[14]?.saturdayHours,
			day16saturday: this.editingRow?.workingTimeRecordsAggregated[15]?.saturdayHours,
			day17saturday: this.editingRow?.workingTimeRecordsAggregated[16]?.saturdayHours,
			day18saturday: this.editingRow?.workingTimeRecordsAggregated[17]?.saturdayHours,
			day19saturday: this.editingRow?.workingTimeRecordsAggregated[18]?.saturdayHours,
			day20saturday: this.editingRow?.workingTimeRecordsAggregated[19]?.saturdayHours,
			day21saturday: this.editingRow?.workingTimeRecordsAggregated[20]?.saturdayHours,
			day22saturday: this.editingRow?.workingTimeRecordsAggregated[21]?.saturdayHours,
			day23saturday: this.editingRow?.workingTimeRecordsAggregated[22]?.saturdayHours,
			day24saturday: this.editingRow?.workingTimeRecordsAggregated[23]?.saturdayHours,
			day25saturday: this.editingRow?.workingTimeRecordsAggregated[24]?.saturdayHours,
			day26saturday: this.editingRow?.workingTimeRecordsAggregated[25]?.saturdayHours,
			day27saturday: this.editingRow?.workingTimeRecordsAggregated[26]?.saturdayHours,
			day28saturday: this.editingRow?.workingTimeRecordsAggregated[27]?.saturdayHours,
			day29saturday: this.editingRow?.workingTimeRecordsAggregated[28]?.saturdayHours,
			day30saturday: this.editingRow?.workingTimeRecordsAggregated[29]?.saturdayHours,
			day31saturday: this.editingRow?.workingTimeRecordsAggregated[30]?.saturdayHours,

			day1night: this.editingRow?.workingTimeRecordsAggregated[0]?.nightHours,
			day2night: this.editingRow?.workingTimeRecordsAggregated[1]?.nightHours,
			day3night: this.editingRow?.workingTimeRecordsAggregated[2]?.nightHours,
			day4night: this.editingRow?.workingTimeRecordsAggregated[3]?.nightHours,
			day5night: this.editingRow?.workingTimeRecordsAggregated[4]?.nightHours,
			day6night: this.editingRow?.workingTimeRecordsAggregated[5]?.nightHours,
			day7night: this.editingRow?.workingTimeRecordsAggregated[6]?.nightHours,
			day8night: this.editingRow?.workingTimeRecordsAggregated[7]?.nightHours,
			day9night: this.editingRow?.workingTimeRecordsAggregated[8]?.nightHours,
			day10night: this.editingRow?.workingTimeRecordsAggregated[9]?.nightHours,
			day11night: this.editingRow?.workingTimeRecordsAggregated[10]?.nightHours,
			day12night: this.editingRow?.workingTimeRecordsAggregated[11]?.nightHours,
			day13night: this.editingRow?.workingTimeRecordsAggregated[12]?.nightHours,
			day14night: this.editingRow?.workingTimeRecordsAggregated[13]?.nightHours,
			day15night: this.editingRow?.workingTimeRecordsAggregated[14]?.nightHours,
			day16night: this.editingRow?.workingTimeRecordsAggregated[15]?.nightHours,
			day17night: this.editingRow?.workingTimeRecordsAggregated[16]?.nightHours,
			day18night: this.editingRow?.workingTimeRecordsAggregated[17]?.nightHours,
			day19night: this.editingRow?.workingTimeRecordsAggregated[18]?.nightHours,
			day20night: this.editingRow?.workingTimeRecordsAggregated[19]?.nightHours,
			day21night: this.editingRow?.workingTimeRecordsAggregated[20]?.nightHours,
			day22night: this.editingRow?.workingTimeRecordsAggregated[21]?.nightHours,
			day23night: this.editingRow?.workingTimeRecordsAggregated[22]?.nightHours,
			day24night: this.editingRow?.workingTimeRecordsAggregated[23]?.nightHours,
			day25night: this.editingRow?.workingTimeRecordsAggregated[24]?.nightHours,
			day26night: this.editingRow?.workingTimeRecordsAggregated[25]?.nightHours,
			day27night: this.editingRow?.workingTimeRecordsAggregated[26]?.nightHours,
			day28night: this.editingRow?.workingTimeRecordsAggregated[27]?.nightHours,
			day29night: this.editingRow?.workingTimeRecordsAggregated[28]?.nightHours,
			day30night: this.editingRow?.workingTimeRecordsAggregated[29]?.nightHours,
			day31night: this.editingRow?.workingTimeRecordsAggregated[30]?.nightHours
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

		const formModel = this.detailedHoursForm.getRawValue();
		const updateModel = new WorkingTimeRecordDetailedDataFormModel({
			employeeId: formModel.employeeId,
			year: formModel.year,
			month: formModel.month,
			dayHours: {
				1: formModel.day1,
				2: formModel.day2,
				3: formModel.day3,
				4: formModel.day4,
				5: formModel.day5,
				6: formModel.day6,
				7: formModel.day7,
				8: formModel.day8,
				9: formModel.day9,
				10: formModel.day10,
				11: formModel.day11,
				12: formModel.day12,
				13: formModel.day13,
				14: formModel.day14,
				15: formModel.day15,
				16: formModel.day16,
				17: formModel.day17,
				18: formModel.day18,
				19: formModel.day19,
				20: formModel.day20,
				21: formModel.day21,
				22: formModel.day22,
				23: formModel.day23,
				24: formModel.day24,
				25: formModel.day25,
				26: formModel.day26,
				27: formModel.day27,
				28: formModel.day28,
				29: formModel.day29,
				30: formModel.day30,
				31: formModel.day31
			},
			saturdayHours: {
				1: formModel.day1saturday,
				2: formModel.day2saturday,
				3: formModel.day3saturday,
				4: formModel.day4saturday,
				5: formModel.day5saturday,
				6: formModel.day6saturday,
				7: formModel.day7saturday,
				8: formModel.day8saturday,
				9: formModel.day9saturday,
				10: formModel.day10saturday,
				11: formModel.day11saturday,
				12: formModel.day12saturday,
				13: formModel.day13saturday,
				14: formModel.day14saturday,
				15: formModel.day15saturday,
				16: formModel.day16saturday,
				17: formModel.day17saturday,
				18: formModel.day18saturday,
				19: formModel.day19saturday,
				20: formModel.day20saturday,
				21: formModel.day21saturday,
				22: formModel.day22saturday,
				23: formModel.day23saturday,
				24: formModel.day24saturday,
				25: formModel.day25saturday,
				26: formModel.day26saturday,
				27: formModel.day27saturday,
				28: formModel.day28saturday,
				29: formModel.day29saturday,
				30: formModel.day30saturday,
				31: formModel.day31saturday
			},
			nightHours: {
				1: formModel.day1night,
				2: formModel.day2night,
				3: formModel.day3night,
				4: formModel.day4night,
				5: formModel.day5night,
				6: formModel.day6night,
				7: formModel.day7night,
				8: formModel.day8night,
				9: formModel.day9night,
				10: formModel.day10night,
				11: formModel.day11night,
				12: formModel.day12night,
				13: formModel.day13night,
				14: formModel.day14night,
				15: formModel.day15night,
				16: formModel.day16night,
				17: formModel.day17night,
				18: formModel.day18night,
				19: formModel.day19night,
				20: formModel.day20night,
				21: formModel.day21night,
				22: formModel.day22night,
				23: formModel.day23night,
				24: formModel.day24night,
				25: formModel.day25night,
				26: formModel.day26night,
				27: formModel.day27night,
				28: formModel.day28night,
				29: formModel.day29night,
				30: formModel.day30night,
				31: formModel.day31night
			}
		});

		this.store.dispatch(new UpdateDetailedData(updateModel.employeeId, updateModel));
		this.cancelEditMode();
	}

	onPrint(divIdName: string): void {
		this.store.dispatch(new PrintData(divIdName));
	}
}
