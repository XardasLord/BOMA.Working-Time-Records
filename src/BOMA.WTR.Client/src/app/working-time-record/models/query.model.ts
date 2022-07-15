import { DepartmentsArray } from '../../shared/models/departments-array';
import { DepartmentsEnum } from '../../shared/models/departments.enum';

export class QueryModel {
	public searchText!: string;
	public year!: number;
	public month!: number;
	public departmentId!: number;
}

export const DefaultQueryModel: QueryModel = {
	searchText: '',
	year: new Date().getFullYear(),
	month: new Date().getMonth() + 1,
	departmentId: DepartmentsArray.filter((x) => x.key === DepartmentsEnum.BomaDepartment)[0].value
};
