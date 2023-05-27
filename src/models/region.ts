export interface IRegionPreview {
  id: string;
  saRegionId?: number;
  nameRo: string;
  nameRu: string;
  regionTypeId: string;
  statisticCode: string;
  validFrom: string;
  validTo: string;
}

export interface IRegionsPreview {
  data?: any;
  items: any;
}
