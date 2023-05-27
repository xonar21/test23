export interface IElectionImport {
  electionId: number;
  id?: string;
  nameRo?: string;
  nameRu?: string;
  status?: number;
  startCollectingDate?: string;
  endCollectingDate?: string;
}

export interface IElectionPreview {
  data: any;
  items?: any;
  elections?: any;
}
