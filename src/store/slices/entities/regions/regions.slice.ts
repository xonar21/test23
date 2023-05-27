import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { webApi } from "~/api";
import { IRegionPreview, IRegionsPreview } from "~/models";
import { apiCall } from "~/store";

const name = "REGIONS";

export const regionsSliceInitialState: IRegionsPreview = {
  items: null,
};

enum ActionType {
  REGIONS_REQUEST_SUCCEEDED = "REGIONS_REQUEST_SUCCEEDED",
}

const slice = createSlice({
  name,
  initialState: regionsSliceInitialState,
  reducers: {
    [ActionType.REGIONS_REQUEST_SUCCEEDED]: (state, action: PayloadAction<IRegionsPreview>) => {
      state.items = action.payload.items as IRegionsPreview;
    },
  },
});

const { REGIONS_REQUEST_SUCCEEDED } = slice.actions;

const getRegions = (paramsPayload?: any) => {
  return apiCall(webApi.Regions.getList)({
    args: [
      paramsPayload
        ? {
            PageNumber: paramsPayload.number,
            PageSize: paramsPayload.size,
            Filters: paramsPayload.filters,
            SortField: paramsPayload.sortField,
            SortOrder: paramsPayload.sortOrder,
          }
        : {},
    ],
    success: REGIONS_REQUEST_SUCCEEDED.type,
  });
};

const updateRegions = (region: IRegionPreview) => {
  return apiCall(webApi.Regions.updateRegions)({
    args: [region],
  });
};

export const RegionsActions = {
  ...slice.actions,
  getRegions,
  updateRegions,
};

export default slice.reducer;
