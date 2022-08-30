import { KeyValuePair } from '../../working-time-record/models/key-value-pair.model';
import { DepartmentsEnum } from './departments.enum';

export const DepartmentsArray: KeyValuePair<DepartmentsEnum, number>[] = [
	{ key: DepartmentsEnum.All, value: 0 },
	{ key: DepartmentsEnum.WarehouseDepartment, value: 1 },
	{ key: DepartmentsEnum.AccessoryDepartment, value: 2 },
	{ key: DepartmentsEnum.ProductionDepartment, value: 3 },
	{ key: DepartmentsEnum.PackingDepartment, value: 4 },
	{ key: DepartmentsEnum.BomaDepartment, value: 6 },
	{ key: DepartmentsEnum.OrderDepartment, value: 7 },
	{ key: DepartmentsEnum.AgencyDepartment, value: 8 }
];
