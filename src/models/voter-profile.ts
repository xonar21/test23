export interface IVoterProfilePreview {
  rsaPersonId: string;
  idnp: string;
  registrationDate: string;
  email: string;
  lastName: string;
  firstName: string;
  middleName: string;
  dateOfBirth: string;
  genderId: string;
  genderName: string;
  identityNumber: string;
  identitySeries: string;
  personStatusId: string;
  personStatusName: string;
  revision: string;
  disactivationDate: string;
  regionId: string;
  regionName: string;
  localityId: string;
  localityName: string;
  street: string;
  house: string;
  bloc: string;
  apartment: string;
}

export interface IVoterProfilesPreview {
  data?: any;
  history?: any;
  address?: any;
  person?: any;
  items?: any;
  obj?: any;
}
