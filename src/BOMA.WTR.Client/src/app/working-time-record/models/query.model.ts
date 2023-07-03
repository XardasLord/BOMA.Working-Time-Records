import { DepartmentsArray } from '../../shared/models/departments-array';
import { DepartmentsEnum } from '../../shared/models/departments.enum';
import { ShiftTypesArray } from '../../shared/models/shift-types-array';
import { ShiftTypesEnum } from '../../shared/models/shift-types.enum';

export class QueryModel {
	public searchText!: string;
	public year!: number;
	public month!: number;
	public departmentId!: number;
	public shiftId!: number;
}

export const DefaultQueryModel: QueryModel = {
	searchText: '',
	year: new Date().getFullYear(),
	month: new Date().getMonth() + 1,
	departmentId: DepartmentsArray.filter((x) => x.key === DepartmentsEnum.BomaDepartment)[0].value,
	shiftId: ShiftTypesArray.filter((x) => x.key === ShiftTypesEnum.All)[0].value
};

export const DefaultColumnsToDisplayForDetailedTable: string[] = [
	'actions',
	'index',
	'fullName',
	'rate',
	'gross',
	'bonus',
	'sumValue',
	'sumBonus',
	'sumHours',
	'emptyLabel'
];

export const DefaultColumnsToDisplayForReportHoursTable: string[] = [
	'index',
	'fullName',
	'shiftType',
	'position',
	'sumHours',
	'emptyLabel'
];

export const DefaultColumnsToDisplayForAbsentReportTable: string[] = ['index', 'fullName', 'shiftType', 'position', 'emptyLabel'];

export function DefaultInitialDayColumnsToDisplayForDetailedTable(year: number, month: number): string[] {
	const daysArray: string[] = [];
	for (let i = 1; i <= NumberOfDaysInMonth(year, month); i++) {
		daysArray.push(i.toString());
	}

	return daysArray;
}

export function NumberOfDaysInMonth(year: number, month: number): number {
	return new Date(year, month, 0).getDate();
}
