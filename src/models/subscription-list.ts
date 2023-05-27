export interface ISubscriptionListCreate {
  lsSubscriptionListId: number;
  nameRo: string;
  nameRu: string;
  genderId: string;
  electionId: string;
  circumscriptionId: string;
  ballotFunctionId: string;
  isIndependentCandidate: boolean;
  politicalPartyId: string;
  idnp: string;
  dateOfBirth: string;
  professionRo: string;
  professionRu: string;
  positionRo: string;
  positionRu: string;
  workPlaceRo: string;
  workPlaceRu: string;
  id: string;
}

export interface ISubscriptionListsPreview {
  data: any;
  items?: any;
}
