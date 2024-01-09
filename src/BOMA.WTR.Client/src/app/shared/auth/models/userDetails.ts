export interface UserDetails {
	id: string;
	email: string;
	role: Role;
	activated: boolean;
}

export enum Role {
	Admin = 'Admin',
	User = 'User'
}
