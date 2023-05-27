import { Permission } from "~/security";

export interface IUserPreview {
  data?: any;
  claims?: { type: Permission; value: string }[];
  id: string;
  email: string;
  userName: string;
  idnp?: string;
}

export interface IUsersPreview {
  data: any;
  items?: any;
}
