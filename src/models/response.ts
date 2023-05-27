export interface IResponse<T> {
  refreshToken?: string;
  token?: string;
  data: T[];
  total: number;
  page: number;
  limit: number;
  totalCount?: number;
  items?: any;
}
