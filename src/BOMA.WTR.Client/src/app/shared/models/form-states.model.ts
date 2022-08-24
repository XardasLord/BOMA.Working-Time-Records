export class FormStateModel<T> {
	model!: T;
	dirty!: boolean;
	status!: string;
	errors: any;
}

export const DefaultFormStateValue: FormStateModel<any> = {
	model: undefined,
	dirty: false,
	status: '',
	errors: {}
};
