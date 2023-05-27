import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { webApi } from "~/api";
import { IVoterProfilesPreview } from "~/models";
import { apiCall } from "~/store";

const name = "VOTER";

export const voterSliceInitialState: IVoterProfilesPreview = {
  address: null,
  person: null,
  data: null,
};

enum ActionType {
  VOTER_REQUEST_SUCCEEDED = "VOTER_REQUEST_SUCCEEDED",
  VOTER_ADRESS_REQUEST_SUCCEEDED = "VOTER_ADRESS_REQUEST_SUCCEEDED",
  VOTER_PERSON_REQUEST_SUCCEEDED = "VOTER_PERSON_REQUEST_SUCCEEDED",
}

const slice = createSlice({
  name,
  initialState: voterSliceInitialState,
  reducers: {
    [ActionType.VOTER_REQUEST_SUCCEEDED]: (state, action: PayloadAction<IVoterProfilesPreview>) => {
      state.data = action.payload as IVoterProfilesPreview;
    },
    [ActionType.VOTER_ADRESS_REQUEST_SUCCEEDED]: (state, action: PayloadAction<IVoterProfilesPreview>) => {
      state.address = action.payload.items as IVoterProfilesPreview;
    },
    [ActionType.VOTER_PERSON_REQUEST_SUCCEEDED]: (state, action: PayloadAction<IVoterProfilesPreview>) => {
      state.person = action.payload.items as IVoterProfilesPreview;
    },
  },
});

const { VOTER_REQUEST_SUCCEEDED, VOTER_ADRESS_REQUEST_SUCCEEDED, VOTER_PERSON_REQUEST_SUCCEEDED } = slice.actions;

const getVoterProfileOwn = () => {
  return apiCall(webApi.VoterProfile.getProfile)({
    args: [{ page: 0 }],
    success: VOTER_REQUEST_SUCCEEDED.type,
  });
};

const getVoterProfiles = (paramsPayload: any) => {
  return apiCall(webApi.VoterProfile.getProfiles)({
    args: [
      {
        PageNumber: paramsPayload.number,
        PageSize: paramsPayload.size,
        Filters: paramsPayload.filters,
        SortField: paramsPayload.sortField,
        SortOrder: paramsPayload.sortOrder,
      },
    ],
    success: VOTER_REQUEST_SUCCEEDED.type,
  });
};

const getVoterProfileIdnp = (idnp: string) => {
  return apiCall(webApi.VoterProfile.getProfileIdnp)({
    args: [{ idnp }],
    success: VOTER_REQUEST_SUCCEEDED.type,
  });
};

const getVoterProfileOwnAdress = (paramsPayload: any) => {
  return apiCall(webApi.VoterProfile.getProfileAdress)({
    args: [
      {
        idnp: paramsPayload.idnp,
        PageNumber: paramsPayload.number,
        PageSize: paramsPayload.size,
        Filters: paramsPayload.filters,
        SortField: paramsPayload.sortField,
        SortOrder: paramsPayload.sortOrder,
      },
    ],
    success: VOTER_ADRESS_REQUEST_SUCCEEDED.type,
  });
};

const getVoterProfileOwnAdressIdnp = (paramsPayload: any) => {
  return apiCall(webApi.VoterProfile.getProfileAdressIdnp)({
    args: [
      {
        idnp: paramsPayload.idnp,
        PageNumber: paramsPayload.number,
        PageSize: paramsPayload.size,
        Filters: paramsPayload.filters,
        SortField: paramsPayload.sortField,
        SortOrder: paramsPayload.sortOrder,
      },
    ],
    success: VOTER_ADRESS_REQUEST_SUCCEEDED.type,
  });
};

const getVoterProfileOwnPerson = (paramsPayload: any) => {
  return apiCall(webApi.VoterProfile.getProfilePerson)({
    args: [
      {
        PageNumber: paramsPayload.number,
        PageSize: paramsPayload.size,
        Filters: paramsPayload.filters,
        SortField: paramsPayload.sortField,
        SortOrder: paramsPayload.sortOrder,
      },
    ],
    success: VOTER_PERSON_REQUEST_SUCCEEDED.type,
  });
};

const getVoterProfileOwnPersonIdnp = (paramsPayload: any) => {
  return apiCall(webApi.VoterProfile.getProfilePersonIdnp)({
    args: [
      {
        idnp: paramsPayload.idnp,
        PageNumber: paramsPayload.number,
        PageSize: paramsPayload.size,
        Filters: paramsPayload.filters,
        SortField: paramsPayload.sortField,
        SortOrder: paramsPayload.sortOrder,
      },
    ],
    success: VOTER_PERSON_REQUEST_SUCCEEDED.type,
  });
};

const postEmailAttach = (params: any) => {
  return apiCall(webApi.VoterProfile.postEmailAttach)({
    args: [
      {
        email: params.email,
      },
    ],
  });
};

const postConfirmAttached = (params: any) => {
  return apiCall(webApi.VoterProfile.postConfirmAttached)({
    args: [params],
  });
};

export const VoterActions = {
  ...slice.actions,
  getVoterProfiles,
  getVoterProfileIdnp,
  getVoterProfileOwn,
  getVoterProfileOwnAdress,
  getVoterProfileOwnAdressIdnp,
  getVoterProfileOwnPerson,
  getVoterProfileOwnPersonIdnp,
  postEmailAttach,
  postConfirmAttached,
};

export default slice.reducer;
