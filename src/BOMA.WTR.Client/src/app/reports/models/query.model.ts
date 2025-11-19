import { DepartmentsArray } from '../../shared/models/departments-array';
import { DepartmentsEnum } from '../../shared/models/departments.enum';
import { ShiftTypesArray } from '../../shared/models/shift-types-array';
import { ShiftTypesEnum } from '../../shared/models/shift-types.enum';

export class QueryModel {
	public searchText!: string;
	public startDate!: Date;
	public endDate!: Date;
	public departmentId!: number;
	public shiftId!: number;
}

const now = new Date();
const startOfMonth = new Date(now.getFullYear(), now.getMonth(), 1);
const endOfMonth = new Date(now.getFullYear(), now.getMonth() + 1, 0);

export const DefaultQueryModel: QueryModel = {
	searchText: '',
	startDate: startOfMonth,
	endDate: endOfMonth,
	departmentId: DepartmentsArray.filter((x) => x.key === DepartmentsEnum.BomaDepartment)[0].value,
	shiftId: ShiftTypesArray.filter((x) => x.key === ShiftTypesEnum.All)[0].value
};
