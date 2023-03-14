export type AuthActionType = "login" | "register";

export interface IAuthResponse {
  token: string;
}

export interface IAuthError {
  error: string;
}

export interface IAuthModel {
  email: string;
  password: string;
}
