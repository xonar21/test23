export interface IRoleCreate {
  name?: string;
  claims?: string[];
  id?: string;
}

export interface IRolePreview extends IRoleCreate {
  data?: any;
  items?: any;
}
