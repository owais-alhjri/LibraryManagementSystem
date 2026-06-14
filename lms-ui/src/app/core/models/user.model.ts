export type Role = 'ADMIN' | 'LIBRARIAN' | 'MEMBER';

export interface User {
  id: string;
  name: string;
  email: string;
  role: Role;
}

export interface UpdateRoleRequest {
  role: Role;
}
