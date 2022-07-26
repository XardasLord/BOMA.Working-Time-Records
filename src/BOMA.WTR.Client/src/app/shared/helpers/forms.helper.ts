import { FormControlOriginalValueValidationModel } from '../validators/form-control-original-value-validation.model';
import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function createValueChangedValidator(originalValueModel: FormControlOriginalValueValidationModel): ValidatorFn {
	return (control: AbstractControl): ValidationErrors | null => {
		let value = control.value;
		let originalValue = originalValueModel.originalValue;

		return null;
		// return value === originalValue ? { valueNotChanged: true } : null;
	};
}
