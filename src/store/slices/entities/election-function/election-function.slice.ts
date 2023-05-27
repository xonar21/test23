import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { webApi } from "~/api";
import { IElectionFunctionCreate, IElectionFunctionPreview, IRoleCreate, IRolePreview } from "~/models";
import { apiCall } from "~/store";

const name = "ELECTION_FUNCTION";

export const electionFunctionSliceInitialState: IElectionFunctionPreview = {
  data: [],
};

enum ActionType {
  ELECTION_FUNCTION_REQUEST_SUCCEEDED = "ELECTION_FUNCTION_REQUEST_SUCCEEDED",
}

const slice = createSlice({
  name,
  initialState: electionFunctionSliceInitialState,
  reducers: {
    [ActionType.ELECTION_FUNCTION_REQUEST_SUCCEEDED]: (state, action: PayloadAction<IElectionFunctionPreview>) => {
      state.data = action.payload.items as IElectionFunctionPreview;
    },
  },
});

const { ELECTION_FUNCTION_REQUEST_SUCCEEDED } = slice.actions;

const getElectionFunctions = (paramsPayload: any) => {
  return apiCall(webApi.ElectionFunction.getList)({
    args: [
      {
        PageNumber: paramsPayload.number,
        PageSize: paramsPayload.size,
        Filters: paramsPayload.filters,
        SortField: paramsPayload.sortField,
        SortOrder: paramsPayload.sortOrder,
      },
    ],
    success: ELECTION_FUNCTION_REQUEST_SUCCEEDED.type,
  });
};
const deleteElectionFunction = (id: string) => {
  return apiCall(webApi.ElectionFunction.deleteElectionFunction)({
    args: [id],
  });
};

const updateElectionFunction = (electionFunction: IElectionFunctionCreate) => {
  return apiCall(webApi.ElectionFunction.updateElectionFunction)({
    args: [electionFunction],
  });
};

const createElectionFunction = (electionFunction: IElectionFunctionCreate) => {
  return apiCall(webApi.ElectionFunction.createElectionFunction)({
    args: [electionFunction],
  });
};

export const ElectionFunctionActions = {
  getElectionFunctions,
  createElectionFunction,
  deleteElectionFunction,
  updateElectionFunction,
};

export default slice.reducer;
