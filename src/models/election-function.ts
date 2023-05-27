export interface IElectionFunctionCreate {
  nameRo: string;
  nameRu: string;
  isElective: boolean;
  id?: string;
}

export interface IElectionFunctionPreview {
  data: any;
  items?: any;
}
