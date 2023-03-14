import { ILocation } from "~/models";

export interface IUserPreview {
  id: string;
  title: string;
  firstName: string;
  lastName: string;
  picture: string;
}

export interface IUser extends IUserPreview {
  gender: string;
  email: string;
  dateOfBirth: string;
  registerDate: string;
  phone: string;
  location: ILocation;
}
