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
