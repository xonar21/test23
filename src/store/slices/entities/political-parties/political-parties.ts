import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { webApi } from "~/api";
import { IPoliticalPartiesPreview, IPoliticalPartiesCreate } from "~/models";
import { apiCall } from "~/store";

const name = "POLITICAL_PARTIES";

export const politicalPartiesSliceInitialState: IPoliticalPartiesPreview = {
  data: [],
};

enum ActionType {
  POLITICAL_PARTIES_REQUEST_SUCCEEDED = "POLITICAL_PARTIES_REQUEST_SUCCEEDED",
}

const slice = createSlice({
  name,
  initialState: politicalPartiesSliceInitialState,
  reducers: {
    [ActionType.POLITICAL_PARTIES_REQUEST_SUCCEEDED]: (state, action: PayloadAction<IPoliticalPartiesPreview>) => {
      state.data = action.payload.items as IPoliticalPartiesPreview;
    },
  },
});

const { POLITICAL_PARTIES_REQUEST_SUCCEEDED } = slice.actions;

const getPoliticalParties = (paramsPayload: any) => {
  return apiCall(webApi.PoliticalParties.getList)({
    args: [
      {
        PageNumber: paramsPayload.number,
        PageSize: paramsPayload.size,
        Filters: paramsPayload.filters,
        SortField: paramsPayload.sortField,
        SortOrder: paramsPayload.sortOrder,
      },
    ],
    success: POLITICAL_PARTIES_REQUEST_SUCCEEDED.type,
  });
};
const deletePoliticalParties = (id: string) => {
  return apiCall(webApi.PoliticalParties.deletePoliticalParties)({
    args: [id],
  });
};

const deactivatePoliticalParties = (politicalParties: any) => {
  return apiCall(webApi.PoliticalParties.deactivatePoliticalParties)({
    args: [politicalParties],
  });
};

const activatePoliticalParties = (politicalParties: any) => {
  return apiCall(webApi.PoliticalParties.activatePoliticalParties)({
    args: [politicalParties],
  });
};

const updatePoliticalParties = (politicalParties: IPoliticalPartiesCreate) => {
  return apiCall(webApi.PoliticalParties.updatePoliticalParties)({
    args: [politicalParties],
  });
};

const createPoliticalParties = (politicalParties: IPoliticalPartiesCreate) => {
  return apiCall(webApi.PoliticalParties.createPoliticalParties)({
    args: [politicalParties],
  });
};

export const PoliticalPartiesActions = {
  getPoliticalParties,
  deletePoliticalParties,
  updatePoliticalParties,
  createPoliticalParties,
  deactivatePoliticalParties,
  activatePoliticalParties,
};

export default slice.reducer;
