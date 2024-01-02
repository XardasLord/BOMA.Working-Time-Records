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
	WorkingTimeRecordReportHoursDataFormModel,
	WorkingTimeRecordReportHoursFormGroup
} from '../../models/working-time-record-summary-data-form.model';
import moment from 'moment';
import UpdateReportHoursData = WorkingTimeRecord.UpdateReportHoursData;

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
		this.reportHoursForm = this.fb.group<WorkingTimeRecordReportHoursFormGroup>({
			year: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			month: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			employeeId: new FormControl(0, { nonNullable: true, validators: [Validators.min(0)] }),
			day1Entry: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day2Entry: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day3Entry: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day4Entry: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day5Entry: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day6Entry: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day7Entry: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day8Entry: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day9Entry: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day10Entry: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day11Entry: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day12Entry: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day13Entry: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day14Entry: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day15Entry: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day16Entry: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day17Entry: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day18Entry: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day19Entry: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day20Entry: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day21Entry: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day22Entry: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day23Entry: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day24Entry: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day25Entry: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day26Entry: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day27Entry: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day28Entry: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day29Entry: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day30Entry: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day31Entry: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),

			day1Exit: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day2Exit: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day3Exit: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day4Exit: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day5Exit: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day6Exit: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day7Exit: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day8Exit: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day9Exit: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day10Exit: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day11Exit: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day12Exit: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day13Exit: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day14Exit: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day15Exit: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day16Exit: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day17Exit: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day18Exit: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day19Exit: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day20Exit: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day21Exit: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day22Exit: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day23Exit: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day24Exit: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day25Exit: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day26Exit: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day27Exit: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day28Exit: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day29Exit: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day30Exit: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] }),
			day31Exit: new FormControl('', { nonNullable: true, validators: [Validators.min(0)] })
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

		const emptyTimeFormat = '';

		// Set initial value for all days
		for (let day = 1; day <= 31; day++) {
			const entry = `day${day}Entry`;
			const exit = `day${day}Exit`;

			const workTimeRecord = this.editingRow?.workingTimeRecordsAggregated[day - 1];

			if (workTimeRecord) {
				this.reportHoursForm.patchValue({
					[entry]:
						workTimeRecord.workTimePeriodOriginal.duration != '00:00:00'
							? moment(workTimeRecord.workTimePeriodOriginal.from).format('HH:mm')
							: emptyTimeFormat,
					[exit]:
						workTimeRecord.workTimePeriodOriginal.duration != '00:00:00'
							? moment(workTimeRecord.workTimePeriodOriginal.to).format('HH:mm')
							: emptyTimeFormat
				});
			}
		}

		this.reportHoursForm.patchValue({
			employeeId: this.editingRow?.employee.id
		});
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

		const reportEntryHours: Record<number, string> = {};
		const reportExitHours: Record<number, string> = {};

		// Loop through each day of the month
		for (let day = 1; day <= 31; day++) {
			// @ts-ignore
			reportEntryHours[day] = formModel[`day${day}Entry` as keyof typeof formModel];

			// @ts-ignore
			reportExitHours[day] = formModel[`day${day}Exit` as keyof typeof formModel];
		}

		// Create the updateModel object using the loop-generated data
		const updateModel = new WorkingTimeRecordReportHoursDataFormModel({
			employeeId: formModel.employeeId,
			year: formModel.year,
			month: formModel.month,
			reportEntryHours: reportEntryHours,
			reportExitHours: reportExitHours
		});

		this.store.dispatch(new UpdateReportHoursData(updateModel.employeeId, updateModel));
		this.cancelEditMode();
	}
}
