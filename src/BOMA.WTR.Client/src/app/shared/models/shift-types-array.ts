import { KeyValuePair } from '../../working-time-record/models/key-value-pair.model';
import { ShiftTypesEnum } from './shift-types.enum';

export const ShiftTypesArray: KeyValuePair<ShiftTypesEnum, number>[] = [
	{ key: ShiftTypesEnum.All, value: 0 },
	{ key: ShiftTypesEnum.First, value: 1 },
	{ key: ShiftTypesEnum.Second, value: 2 },
	{ key: ShiftTypesEnum.Third, value: 3 }
];
