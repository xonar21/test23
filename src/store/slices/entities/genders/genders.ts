import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { webApi } from "~/api";
import { IGendersPreview, IGendersUpdate } from "~/models";
import { apiCall } from "~/store";

const name = "GENDERS";

export const gendersSliceInitialState: IGendersPreview = {
  data: [],
};

enum ActionType {
  GENDERS_REQUEST_SUCCEEDED = "GENDERS_REQUEST_SUCCEEDED",
}

const slice = createSlice({
  name,
  initialState: gendersSliceInitialState,
  reducers: {
    [ActionType.GENDERS_REQUEST_SUCCEEDED]: (state, action: PayloadAction<IGendersPreview>) => {
      state.data = action.payload.items as IGendersPreview;
    },
  },
});

const { GENDERS_REQUEST_SUCCEEDED } = slice.actions;

const getGenders = (paramsPayload: any) => {
  return apiCall(webApi.Genders.getList)({
    args: [
      {
        PageNumber: paramsPayload.number,
        PageSize: paramsPayload.size,
        Filters: paramsPayload.filters,
        SortField: paramsPayload.sortField,
        SortOrder: paramsPayload.sortOrder,
      },
    ],
    success: GENDERS_REQUEST_SUCCEEDED.type,
  });
};
const deleteGender = (id: string) => {
  return apiCall(webApi.Genders.deleteGenders)({
    args: [id],
  });
};

const updateGender = (gender: IGendersUpdate) => {
  return apiCall(webApi.Genders.updateGenders)({
    args: [gender],
  });
};

export const GendersActions = {
  getGenders,
  deleteGender,
  updateGender,
};

export default slice.reducer;
