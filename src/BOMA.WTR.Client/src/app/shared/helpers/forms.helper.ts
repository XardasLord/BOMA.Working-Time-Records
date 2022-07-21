import { FormControlOriginalValueValidationModel } from '../../working-time-record/models/FormControlOriginalValueValidationModel';
import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function createValueChangedValidator(originalValueModel: FormControlOriginalValueValidationModel): ValidatorFn {
	return (control: AbstractControl): ValidationErrors | null => {
		let value = control.value;
		let originalValue = originalValueModel.originalValue;

		return value === originalValue ? { valueNotChanged: true } : null;
	};
}
