import { AfterViewInit, Component } from '@angular/core';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { WorkingTimeRecordState } from '../../state/working-time-record.state';
import { EmployeeWorkingTimeRecordDetailsModel } from '../../models/employee-working-time-record-details.model';
import { WorkingTimeRecordDetailsAggregatedModel } from '../../models/working-time-record-details-aggregated.model';
import { WorkingTimeRecord } from '../../state/working-time-record.action';
import SendHoursToGratyfikant = WorkingTimeRecord.SendHoursToGratyfikant;
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import {
	WorkingTimeRecordDetailedDataFormModel,
	WorkingTimeRecordDetailedFormGroup,
	WorkingTimeRecordReportHoursDataFormModel,
	WorkingTimeRecordReportHoursFormGroup
} from '../../models/working-time-record-summary-data-form.model';

@Component({
	selector: 'app-working-time-record-report-hours-table',
	templateUrl: './working-time-record-report-hours-table.component.html',
	styleUrls: ['./working-time-record-report-hours-table.component.scss']
})
export class WorkingTimeRecordReportHoursTableComponent implements AfterViewInit {
	detailedRecords$!: Observable<EmployeeWorkingTimeRecordDetailsModel[]>;
	numberOfDays$ = this.store.select(WorkingTimeRecordState.getDaysInMonth);
	columnsToDisplay$ = this.store.select(WorkingTimeRecordState.getColumnsToDisplayForReportHours);

	reportHoursForm: FormGroup<WorkingTimeRecordReportHoursFormGroup>;
	editingRow: EmployeeWorkingTimeRecordDetailsModel | null = null;

	constructor(private store: Store, private toastService: ToastrService, private fb: FormBuilder) {
		this.reportHoursForm = fb.group<WorkingTimeRecordReportHoursFormGroup>({
			year: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			month: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			employeeId: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day1Entry: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day2Entry: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day3Entry: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day4Entry: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day5Entry: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day6Entry: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day7Entry: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day8Entry: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day9Entry: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day10Entry: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day11Entry: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day12Entry: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day13Entry: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day14Entry: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day15Entry: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day16Entry: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day17Entry: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day18Entry: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day19Entry: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day20Entry: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day21Entry: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day22Entry: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day23Entry: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day24Entry: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day25Entry: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day26Entry: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day27Entry: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day28Entry: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day29Entry: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day30Entry: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day31Entry: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),

			day1Exit: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day2Exit: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day3Exit: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day4Exit: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day5Exit: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day6Exit: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day7Exit: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day8Exit: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day9Exit: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day10Exit: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day11Exit: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day12Exit: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day13Exit: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day14Exit: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day15Exit: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day16Exit: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day17Exit: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day18Exit: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day19Exit: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day20Exit: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day21Exit: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day22Exit: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day23Exit: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day24Exit: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day25Exit: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day26Exit: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day27Exit: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day28Exit: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day29Exit: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day30Exit: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day31Exit: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] })
		});
	}

	ngAfterViewInit() {
		this.detailedRecords$ = this.store.select(WorkingTimeRecordState.getReportHourRecordsNormalizedForTable);
	}

	trackRecord(index: number, element: EmployeeWorkingTimeRecordDetailsModel): number {
		return element.employee.id;
	}

	getAllHoursSum(workingTimeRecordDetails: WorkingTimeRecordDetailsAggregatedModel[]) {
		return workingTimeRecordDetails.reduce<number>((accumulator, obj) => accumulator + obj.workedHoursRounded + obj.saturdayHours, 0);
	}

	getTotalHoursSum() {
		return this.detailedRecords$?.pipe(
			map(
				(results) =>
					results
						.map((r) =>
							r.workingTimeRecordsAggregated.reduce(
								(accumulator, obj) => accumulator + obj.workedHoursRounded + obj.saturdayHours,
								0
							)
						)
						.reduce((acc, obj) => acc + obj, 0) / 3
			)
		);
	}

	sendHoursToGratyfikant() {
		this.store.dispatch(new SendHoursToGratyfikant());
	}

	isRowEditing(row: EmployeeWorkingTimeRecordDetailsModel): boolean {
		return row === this.editingRow;
	}

	cancelEditMode() {
		this.editingRow = null;
	}

	enableEditMode(record: EmployeeWorkingTimeRecordDetailsModel) {
		if (!record.isEditable) {
			this.toastService.warning('Ten rekord nie jest edytowalny');
			return;
		}

		this.editingRow = record;

		// Set initial value for all days
		// this.reportHoursForm.patchValue({
		// 	employeeId: this.editingRow?.employee.id,
		// 	day1Entry: this.editingRow?.workingTimeRecordsAggregated[0]?.workTimePeriodOriginal.from,
		// 	day2Entry: this.editingRow?.workingTimeRecordsAggregated[1]?.workTimePeriodOriginal.from,
		// 	day3Entry: this.editingRow?.workingTimeRecordsAggregated[2]?.workTimePeriodOriginal.from,
		// 	day4Entry: this.editingRow?.workingTimeRecordsAggregated[3]?.workTimePeriodOriginal.from,
		// 	day5Entry: this.editingRow?.workingTimeRecordsAggregated[4]?.workTimePeriodOriginal.from,
		// 	day6Entry: this.editingRow?.workingTimeRecordsAggregated[5]?.workTimePeriodOriginal.from,
		// 	day7Entry: this.editingRow?.workingTimeRecordsAggregated[6]?.workTimePeriodOriginal.from,
		// 	day8Entry: this.editingRow?.workingTimeRecordsAggregated[7]?.workTimePeriodOriginal.from,
		// 	day9Entry: this.editingRow?.workingTimeRecordsAggregated[8]?.workTimePeriodOriginal.from,
		// 	day10Entry: this.editingRow?.workingTimeRecordsAggregated[9]?.workTimePeriodOriginal.from,
		// 	day11Entry: this.editingRow?.workingTimeRecordsAggregated[10]?.workTimePeriodOriginal.from,
		// 	day12Entry: this.editingRow?.workingTimeRecordsAggregated[11]?.workTimePeriodOriginal.from,
		// 	day13Entry: this.editingRow?.workingTimeRecordsAggregated[12]?.workTimePeriodOriginal.from,
		// 	day14Entry: this.editingRow?.workingTimeRecordsAggregated[13]?.workTimePeriodOriginal.from,
		// 	day15Entry: this.editingRow?.workingTimeRecordsAggregated[14]?.workTimePeriodOriginal.from,
		// 	day16Entry: this.editingRow?.workingTimeRecordsAggregated[15]?.workTimePeriodOriginal.from,
		// 	day17Entry: this.editingRow?.workingTimeRecordsAggregated[16]?.workTimePeriodOriginal.from,
		// 	day18Entry: this.editingRow?.workingTimeRecordsAggregated[17]?.workTimePeriodOriginal.from,
		// 	day19Entry: this.editingRow?.workingTimeRecordsAggregated[18]?.workTimePeriodOriginal.from,
		// 	day20Entry: this.editingRow?.workingTimeRecordsAggregated[19]?.workTimePeriodOriginal.from,
		// 	day21Entry: this.editingRow?.workingTimeRecordsAggregated[20]?.workTimePeriodOriginal.from,
		// 	day22Entry: this.editingRow?.workingTimeRecordsAggregated[21]?.workTimePeriodOriginal.from,
		// 	day23Entry: this.editingRow?.workingTimeRecordsAggregated[22]?.workTimePeriodOriginal.from,
		// 	day24Entry: this.editingRow?.workingTimeRecordsAggregated[23]?.workTimePeriodOriginal.from,
		// 	day25Entry: this.editingRow?.workingTimeRecordsAggregated[24]?.workTimePeriodOriginal.from,
		// 	day26Entry: this.editingRow?.workingTimeRecordsAggregated[25]?.workTimePeriodOriginal.from,
		// 	day27Entry: this.editingRow?.workingTimeRecordsAggregated[26]?.workTimePeriodOriginal.from,
		// 	day28Entry: this.editingRow?.workingTimeRecordsAggregated[27]?.workTimePeriodOriginal.from,
		// 	day29Entry: this.editingRow?.workingTimeRecordsAggregated[28]?.workTimePeriodOriginal.from,
		// 	day30Entry: this.editingRow?.workingTimeRecordsAggregated[29]?.workTimePeriodOriginal.from,
		// 	day31Entry: this.editingRow?.workingTimeRecordsAggregated[30]?.workTimePeriodOriginal.from,
		//
		// 	day1Exit: this.editingRow?.workingTimeRecordsAggregated[0]?.workTimePeriodOriginal.to,
		// 	day2Exit: this.editingRow?.workingTimeRecordsAggregated[1]?.workTimePeriodOriginal.to,
		// 	day3Exit: this.editingRow?.workingTimeRecordsAggregated[2]?.workTimePeriodOriginal.to,
		// 	day4Exit: this.editingRow?.workingTimeRecordsAggregated[3]?.workTimePeriodOriginal.to,
		// 	day5Exit: this.editingRow?.workingTimeRecordsAggregated[4]?.workTimePeriodOriginal.to,
		// 	day6Exit: this.editingRow?.workingTimeRecordsAggregated[5]?.workTimePeriodOriginal.to,
		// 	day7Exit: this.editingRow?.workingTimeRecordsAggregated[6]?.workTimePeriodOriginal.to,
		// 	day8Exit: this.editingRow?.workingTimeRecordsAggregated[7]?.workTimePeriodOriginal.to,
		// 	day9Exit: this.editingRow?.workingTimeRecordsAggregated[8]?.workTimePeriodOriginal.to,
		// 	day10Exit: this.editingRow?.workingTimeRecordsAggregated[9]?.workTimePeriodOriginal.to,
		// 	day11Exit: this.editingRow?.workingTimeRecordsAggregated[10]?.workTimePeriodOriginal.to,
		// 	day12Exit: this.editingRow?.workingTimeRecordsAggregated[11]?.workTimePeriodOriginal.to,
		// 	day13Exit: this.editingRow?.workingTimeRecordsAggregated[12]?.workTimePeriodOriginal.to,
		// 	day14Exit: this.editingRow?.workingTimeRecordsAggregated[13]?.workTimePeriodOriginal.to,
		// 	day15Exit: this.editingRow?.workingTimeRecordsAggregated[14]?.workTimePeriodOriginal.to,
		// 	day16Exit: this.editingRow?.workingTimeRecordsAggregated[15]?.workTimePeriodOriginal.to,
		// 	day17Exit: this.editingRow?.workingTimeRecordsAggregated[16]?.workTimePeriodOriginal.to,
		// 	day18Exit: this.editingRow?.workingTimeRecordsAggregated[17]?.workTimePeriodOriginal.to,
		// 	day19Exit: this.editingRow?.workingTimeRecordsAggregated[18]?.workTimePeriodOriginal.to,
		// 	day20Exit: this.editingRow?.workingTimeRecordsAggregated[19]?.workTimePeriodOriginal.to,
		// 	day21Exit: this.editingRow?.workingTimeRecordsAggregated[20]?.workTimePeriodOriginal.to,
		// 	day22Exit: this.editingRow?.workingTimeRecordsAggregated[21]?.workTimePeriodOriginal.to,
		// 	day23Exit: this.editingRow?.workingTimeRecordsAggregated[22]?.workTimePeriodOriginal.to,
		// 	day24Exit: this.editingRow?.workingTimeRecordsAggregated[23]?.workTimePeriodOriginal.to,
		// 	day25Exit: this.editingRow?.workingTimeRecordsAggregated[24]?.workTimePeriodOriginal.to,
		// 	day26Exit: this.editingRow?.workingTimeRecordsAggregated[25]?.workTimePeriodOriginal.to,
		// 	day27Exit: this.editingRow?.workingTimeRecordsAggregated[26]?.workTimePeriodOriginal.to,
		// 	day28Exit: this.editingRow?.workingTimeRecordsAggregated[27]?.workTimePeriodOriginal.to,
		// 	day29Exit: this.editingRow?.workingTimeRecordsAggregated[28]?.workTimePeriodOriginal.to,
		// 	day30Exit: this.editingRow?.workingTimeRecordsAggregated[29]?.workTimePeriodOriginal.to,
		// 	day31Exit: this.editingRow?.workingTimeRecordsAggregated[30]?.workTimePeriodOriginal.to
		// });
	}

	onFormSubmit() {
		if (!this.reportHoursForm.valid) {
			this.toastService.warning('Formularz zawiera nieprawidłowe dane');
			return;
		}

		const query = this.store.selectSnapshot(WorkingTimeRecordState.getSearchQueryModel);

		this.reportHoursForm.patchValue({
			year: query.year,
			month: query.month
		});

		const formModel = this.reportHoursForm.getRawValue();
		//   const updateModel = new WorkingTimeRecordDetailedDataFormModel({
		//     employeeId: formModel.employeeId,
		//     year: formModel.year,
		//     month: formModel.month,
		//     dayHours: {
		//       1: formModel.day1,
		//       2: formModel.day2,
		//       3: formModel.day3,
		//       4: formModel.day4,
		//       5: formModel.day5,
		//       6: formModel.day6,
		//       7: formModel.day7,
		//       8: formModel.day8,
		//       9: formModel.day9,
		//       10: formModel.day10,
		//       11: formModel.day11,
		//       12: formModel.day12,
		//       13: formModel.day13,
		//       14: formModel.day14,
		//       15: formModel.day15,
		//       16: formModel.day16,
		//       17: formModel.day17,
		//       18: formModel.day18,
		//       19: formModel.day19,
		//       20: formModel.day20,
		//       21: formModel.day21,
		//       22: formModel.day22,
		//       23: formModel.day23,
		//       24: formModel.day24,
		//       25: formModel.day25,
		//       26: formModel.day26,
		//       27: formModel.day27,
		//       28: formModel.day28,
		//       29: formModel.day29,
		//       30: formModel.day30,
		//       31: formModel.day31
		//     },
		//     saturdayHours: {
		//       1: formModel.day1saturday,
		//       2: formModel.day2saturday,
		//       3: formModel.day3saturday,
		//       4: formModel.day4saturday,
		//       5: formModel.day5saturday,
		//       6: formModel.day6saturday,
		//       7: formModel.day7saturday,
		//       8: formModel.day8saturday,
		//       9: formModel.day9saturday,
		//       10: formModel.day10saturday,
		//       11: formModel.day11saturday,
		//       12: formModel.day12saturday,
		//       13: formModel.day13saturday,
		//       14: formModel.day14saturday,
		//       15: formModel.day15saturday,
		//       16: formModel.day16saturday,
		//       17: formModel.day17saturday,
		//       18: formModel.day18saturday,
		//       19: formModel.day19saturday,
		//       20: formModel.day20saturday,
		//       21: formModel.day21saturday,
		//       22: formModel.day22saturday,
		//       23: formModel.day23saturday,
		//       24: formModel.day24saturday,
		//       25: formModel.day25saturday,
		//       26: formModel.day26saturday,
		//       27: formModel.day27saturday,
		//       28: formModel.day28saturday,
		//       29: formModel.day29saturday,
		//       30: formModel.day30saturday,
		//       31: formModel.day31saturday
		//     },
		//     nightHours: {
		//       1: formModel.day1night,
		//       2: formModel.day2night,
		//       3: formModel.day3night,
		//       4: formModel.day4night,
		//       5: formModel.day5night,
		//       6: formModel.day6night,
		//       7: formModel.day7night,
		//       8: formModel.day8night,
		//       9: formModel.day9night,
		//       10: formModel.day10night,
		//       11: formModel.day11night,
		//       12: formModel.day12night,
		//       13: formModel.day13night,
		//       14: formModel.day14night,
		//       15: formModel.day15night,
		//       16: formModel.day16night,
		//       17: formModel.day17night,
		//       18: formModel.day18night,
		//       19: formModel.day19night,
		//       20: formModel.day20night,
		//       21: formModel.day21night,
		//       22: formModel.day22night,
		//       23: formModel.day23night,
		//       24: formModel.day24night,
		//       25: formModel.day25night,
		//       26: formModel.day26night,
		//       27: formModel.day27night,
		//       28: formModel.day28night,
		//       29: formModel.day29night,
		//       30: formModel.day30night,
		//       31: formModel.day31night
		//     }
		//   });

		//   this.store.dispatch(new UpdateDetailedData(updateModel.employeeId, updateModel));
		this.cancelEditMode();
	}
}
