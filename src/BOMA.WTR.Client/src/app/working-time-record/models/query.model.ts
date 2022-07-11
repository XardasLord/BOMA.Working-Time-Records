export class QueryModel {
	public searchText!: string;
	public year!: number;
	public month!: number;
	public groupId!: number;
}

export const DefaultQueryModel: QueryModel = {
	searchText: '',
	year: new Date().getFullYear(),
	month: new Date().getMonth() + 1,
	groupId: 6
};
