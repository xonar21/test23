export type AuthActionType = "login" | "register";

export interface IAuthResponse {
  token: string;
  data?: { token?: string; refreshToken?: string };
  SaiseToken?: string;
  refreshToken: string;
}

export interface IAuthError {
  error: string;
}

export interface IAuthModel {
  userName: string;
  password: string;
}

export interface ISaiseModel {
  token: string;
}

export interface IRefreshModel {
  token: string;
  refreshToken: string;
}

export interface IMpassModel {
  requestId: string;
  idnp: string;
  firstName: string;
  nameIdentifier: string;
  lastName: string;
  phoneNumber: string;
  email: string;
  gender: string;
  birthDate: string;
}
export interface IMpassResponse {
  url?: string;
  statusCode?: number;
  value?: string;
}
