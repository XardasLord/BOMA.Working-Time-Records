import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'emptyIfZero' })
export class EmptyIfZeroPipe implements PipeTransform {
	transform(value: any): any {
		return value === 0 ? '' : value;
	}
}
