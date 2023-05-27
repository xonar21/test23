export interface IPoliticalPartiesCreate {
  id?: string;
  saPoliticalPartyId: string;
  code: string;
  nameRo: string;
  nameRu: string;
  shortNameRo: string;
  isElectoralBlocK: string;
}

export interface IPoliticalPartiesPreview {
  data: any;
  items?: any;
}
